# Introducción

En el siguiente documento se muestra un paso a paso de cómo ejecutar un programa de reconocimiento de imágenes en un video mediante la utilización de los servicios Computer Vision y Face API que brinda Microsoft Azure. De los cuales, es necesario indicar el o los atributos que desea que devuelva el servicio; entre las opciones que brinda el servicio de Computer Vision seleccionamos el parámetro adult que devuelve un booleano de si contiene o no contenido adulto, picante o sangriento y su respectiva probabilidad de confidencia; en cuanto al servicio de Face API se seleccionó el parámetro gender que devuelve el género (hombre o mujer). La funcionalidad del servicio de Computer Vision podría ser como una versión mejorada del proyecto anterior, el cual tenía como objetivo ayudar al problema de reconocimiento de imágenes sensibles en una película. 

Se espera que este programa pueda generar un gráfico en donde se muestre en qué tiempo específico de la película se encuentran estas imágenes sensibles, para que los usuarios puedan tomar acción como consideren una vez tienen esta información.


# Instrucciones

## Preparación de la cuenta de Azure y sus servicios 

A continuación se presenta un tutorial de cómo preparar la cuenta de Azure con el servicio Face API https://pablitopiova.medium.com/microsoft-azure-cognitive-services-face-api-2e4cc3161199
Nota: También es necesario realizar el mismo proceso para el servicio de Computer Vision.

## Capturando cambio de escena con ffmpeg

Ahora se nos presenta un problema, ya que se envían todos los frames que posee un video realizaremos demasiadas peticiones a Azure y como contamos con una prueba gratuita, esta tiene ciertas restricciones, por lo que no nos podemos dar este lujo. Debido a esto tenemos que escoger qué frames del video queremos enviarle a Azure para que procese, por lo que necesitamos algo que pueda diferenciar cuando los frames son lo suficientemente distintos para que se considere como que una nueva imagen acaba de aparecer. Para poder lograr esto se utiliza el software ffmpeg, el cual se puede descargar del siguiente enlace https://www.gyan.dev/ffmpeg/builds/ desde el apartado de release builds y full build, ponerlo donde quiera y agregarlo a variables de entorno, para utilizarlo nada más se tiene que ejecutar el siguiente comando:

```
ffmpeg -i {videoPath} -vf \"select = gt(scene\\, 0.5), scale = 640:360\" -vsync vfr -frame_pts 1 %03d.png
```

Este comando permite guardar todos aquellos frames del video donde haya una diferencia mayor al 50% entre ellos, además el parametro -frame_pts permite que las imagenes del resultado sean guardadas con el número del frame en el que fueron capturadas.

## Crear proyecto en C#

Con los pasos anteriores listos, lo siguiente es crear un nuevo proyecto en C#, para esto se debe seleccionar Aplicación de Windows Forms (.NET Framework).

Una vez creado el proyecto es necesario instalar los siguientes paquetes con NuGet:

* Microsoft.Azure.CognitiveServices.Vision.ComputerVision
* Microsoft.Azure.CognitiveServices.Vision.Face
* Microsoft.WindowsAPICodePack.Shell

## Creación de UI

En esta etapa es necesaria la creación del principal punto de entrada de la apliación. Para esto se requiere de una UI que permita al usuario cargar un video y visualizar los resultados del procesamiento mediante dos gráficos, uno que permita ver el porcentaje de escencas de escenas de adultos y otro que permita ver el procentaje de hombres y mujeres, además de dos labels en los que se pueda visualizar la duración de los procesos.

Para esto se puede utilizar la siguiente propuesta de diseño:

![ui](images/ui.png)

Donde se tienen los siguientes componentes:

* chartMaleFemale (el gráfico del % de hombres y mujeres)
* charAdult (el gráfico del % de escenas de adultos)
* dgvMaleFemale (la tabla con las detecciones de hombres y mujeres)
* dgvAdult (la tabla con las detecciones escenas para adultos)
* lblSecuential (label para mostrar el tiempo de la ejecución secuencial)
* lblParallel (label para mostrar el tiempo de la paralela)
* lblVideo (label para mostrar el nombre del video cargado)
* btnLoad (botón de carga de video)
* btnRun (botón para iniciar la ejecución del procesamiento)
* openFileDialog (diálogo para abrir archivos)

## Creación de las clases VisionAPI y FaceAPI

Primero se crea un clase llamada VisionAPI, la cual sirve como contenedor para los métodos del servicio Vision de Azure.

Esta clase debe tener consigo la siguientes variables:

```csharp
private const string SUBSCRIPTION_KEY = "YOUR KEY";
private const string ENDPOINT = "YOUR ENDPOINT";
private ComputerVisionClient computerVision;
private List<VisualFeatureTypes?> features;
```

En SUBSCRIPTION_KEY y ENDPOINT se deben colocar API Key y el endpoint del servicio Vision creado en Azure, computerVision es la instancia de un objeto que permite conectarse con el servicio en Azure y features hace referencia a la variables que se quieren usar como analisis.

Una vez se tiene esto lo siguiente es crear un constructor que cree el objeto computerVision con los datos requeridos.

```csharp
public VisionAPI(List<VisualFeatureTypes?> features)
{
    computerVision =
        new ComputerVisionClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY))
        { Endpoint = ENDPOINT };
    this.features = features;
}
```

También es necesario un método que permita hacer la petición de analisis al servidor. Este metodo debe recibir la imagen convertida a bits (para esto se hace uso de la clase Stream).

```csharp
public Task<ImageAnalysis> StartRecognizing(Stream image)
{
    return computerVision.AnalyzeImageInStreamAsync(image, features);
}
```

Una vez se tenga creada la clase anterior es necesario seguir pasos similares a los anteriores pero esta vez para una clase llamada FaceAPI.

```csharp
private const string SUBSCRIPTION_KEY = "YOUR KEY";
private const string ENDPOINT = "YOUR ENDPOINT";
private FaceClient faceClient;
private List<FaceAttributeType> attributeTypes;

public FaceAPI(List<FaceAttributeType> attributeTypes)
{
    faceClient =
        new FaceClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY))
        { Endpoint = ENDPOINT };
    this.attributeTypes = attributeTypes;
}

public Task<IList<DetectedFace>> StartRecognizing(Stream image)
{
    return faceClient.Face.DetectWithStreamAsync(
        image,
        returnFaceAttributes: attributeTypes,
        recognitionModel: RecognitionModel.Recognition04
    );
}
```

## Código del form

A continuación se presenta el código principal de la apliación.

### Carga del archivo

Primeramente se tiene la sección de carga del archivo, para esto es necesaria una variable global para alamacenar la ruta del archivo que está cargado actualmente.

```csharp
private string videoPath = null;
```

En el evento de click del botón de carga se abre el diálogo de carga, se verfica si se seleccionó una archivo y se guarda en la variable global, además se actualiza el label del archivo cargado para que contega el nombre del archivo.

```csharp
private void btnLoad_Click(object sender, EventArgs e)
{
    DialogResult result = openFileDialog.ShowDialog();
    if (result == DialogResult.OK)
    {
        videoPath = openFileDialog.FileName;
        lblFile.Text = Path.GetFileName(videoPath);
    }
}
```

### Configuración de los gráficos

Primero es necesario definir la variables globales que se van a usar para los gráficos.

```csharp
private int adultFrames; // Total de escenas para adultos
private int maleFrames; // Total de hombres en todas las escenas
private int femaleFrames; // Total de mujeres en todas las escenas
private int totalFrames; // Total de imágenes a analizar
```

Luego se crean dos metodos que permita actualizar los gráficos con sus respectivas variables. En ambos primeramente se debe limpiar el gráfico en caso de que ya hayan datos existentes. Posteriormente se definen los nombres y los valores para los datos del gráfico. Se agrega el titulo y se insertan los nombres y los valores en el gráfico.

```csharp
private void ProccessFaceOutput()
{
    chartMaleFemale.Titles.Clear();
    chartMaleFemale.Series["Series1"].Points.Clear();

    string[] names = { "Hombres", "Mujeres" };
    int[] values = { maleFrames, femaleFrames };
    chartMaleFemale.Titles.Add("Cantidad de Hombres y Mujeres");
    for (int i = 0; i < names.Length; i++)
    {
        chartMaleFemale.Series["Series1"].Points.AddXY(names[i] + " = " + values[i], values[i]);
    }
}

private void ProccessVisionOutput()
{
    chartAdult.Titles.Clear();
    chartAdult.Series["Series1"].Points.Clear();

    string[] names = { "+18", "No +18" };
    int[] values = { adultFrames, totalFrames - adultFrames };
    chartAdult.Titles.Add("Cantidad de escenas para adultos");
    for (int i = 0; i < names.Length; i++)
    {
        chartAdult.Series["Series1"].Points.AddXY(names[i] + " = " + values[i], values[i]);
    }
}
```

### Métodos de utilidad

En primer lugar se ocupa un método que permita crear una carpeta donde se guarden temporalmente las imagenes que se van a procesar. Este método en caso de no existir la carpeta la crea y si ya existe la elimina y la vuelve a crear, de esta manera se eliminan los archivos de detecciones pasadas.

```csharp
private void PrepareImagesFolder()
{
    if (Directory.Exists(IMAGES_FOLDER))
    {
        Directory.Delete(IMAGES_FOLDER, true);
    }
    Directory.CreateDirectory(IMAGES_FOLDER);
}
```

La ruta de la carpeta esta dada por la siguiente variable global

```csharp
private const string IMAGES_FOLDER = @".\images";
```

Los métodos a continuación se utilizan para obtener los FPS de un video y para convertir la posición de un frame a segundos. Estos métodos se utiliza para poder presentar al usuario el segundo del video donde se realizó una detección.

```csharp
private float GetFpsInVideo()
{
    ShellFile shellFile = ShellFile.FromFilePath(videoPath);
    return ((float)shellFile.Properties.System.Video.FrameRate.Value) / 1000;
}

private int FramesToSeconds(int frame, float fps)
{
    return (int) (frame / fps);
}
```

### Decodificado con ffmpeg

Con el fin de no analizar todas las imámgenes se utilizará el programa ffmpeg. Para esto se crea un proceso de cmd y se le pasa el comando con la ruta del video a decodificar y se guardan la imágenes decodificadas en la carpeta de imágenes. Recuerde que el párametro -frame_pts hace que la imágenes se guarden con el número de frame de donde fueron extraídas.

```csharp
private void DecodeKeyImages()
{
    string command = $"/C ffmpeg -i {videoPath} -vf \"select = gt(scene\\, 0.5), scale = 640:360\" -vsync vfr -frame_pts 1 {IMAGES_FOLDER}\\%03d.png";
    Process proc = new Process();
    proc.StartInfo.FileName = "CMD.exe";
    proc.StartInfo.Arguments = command;
    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    proc.Start();
    proc.WaitForExit();
}
```

### Detección

Primero se debe declarar las variables globales que contrendrán las instancias de VisionAPI y FaceAPI.

```csharp
private VisionAPI visionAPI;
private FaceAPI faceAPI;
```

Una vez se tenga eso, se debe crear un método de utilidad el cual permitirá crear las instancias de las variables anteriores. Para esto previamente se configuran los parámetros deseados para la detección. En caso de Vision se quiere detectar si la escena contiene contenido para adultos y en Face el género de una persona.

```csharp
private void InitializeDetectionAPI()
{
    List<VisualFeatureTypes?> visionFeatures = new List<VisualFeatureTypes?>()
    {
        VisualFeatureTypes.Adult
    };
    List<FaceAttributeType> faceAttributes = new List<FaceAttributeType>()
    {
        FaceAttributeType.Gender
    };
    visionAPI = new VisionAPI(visionFeatures);
    faceAPI = new FaceAPI(faceAttributes);
}
```

El siguiente se encargará del análisis de imágenes, para ello recibe la ruta de la imagen. Obtiene el nombre, para saber en que frame está la imagen y hace la conversión de frame a segundos y crea un TimeSpan para más adelante mostrar esta información en HH:MM:SS:ms. 

Una vez se tiene esto se procede a abrir la imagen. En esto caso se abre dos veces, esto debido a que la librerías de Vision y Face cierran la imagen y liberan memoria después de la invocación de sus métodos.

Después se pasa la imagen a los métodos de detección y se obtiene la información y se muestra en las tablas y se refrescan los gráficos.

```csharp
private async Task AnalyzeImage(string imagePath)
{
    int frame = int.Parse(Path.GetFileName(imagePath).Split('.')[0]);
    int seconds = FramesToSeconds(frame, videoFPS);
    TimeSpan time = TimeSpan.FromSeconds(seconds);
    using (Stream imageVision = File.OpenRead(imagePath))
    {
        using (Stream imageFace = File.OpenRead(imagePath))
        {
            ImageAnalysis results = await visionAPI.StartRecognizing(imageVision);
            IList<DetectedFace> detectedFaces = await faceAPI.StartRecognizing(imageFace);
            if ((results.Adult.IsAdultContent || results.Adult.IsRacyContent || results.Adult.IsGoryContent))
            {
                adultFrames++;
                dgvAdult.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"));
                ProccessVisionOutput();
            }
            foreach (DetectedFace face in detectedFaces)
            {
                if (face.FaceAttributes.Gender == 0)
                {
                    maleFrames++;
                    dgvMaleFemale.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"), "Hombre");
                }
                else
                {
                    femaleFrames++;
                    dgvMaleFemale.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"), "Mujer");
                }
                ProccessFaceOutput();
            }
        }
    }
}
```

Este método se encarga de obtener todas la imagenes decodificadas y enviarlas al AnalyzeImage. En primer lugar se tiene la versión con paralelismo de este método que hace uso de la función select la cual permite recorrer la lista y deja que la peticiones se ejecuten en paralelo y después se tiene la versión lineal.

```csharp
private async Task ProcessImages(bool async)
{
    string[] fileEntries = Directory.GetFiles(".\\images");
    totalFrames = fileEntries.Length;
    if (async)
    {
        var tasks = fileEntries.Select(fileName => AnalyzeImage(fileName));
        await Task.WhenAll(tasks);
    }
    else
    {
        foreach (string fileName in fileEntries)
        {
            await AnalyzeImage(fileName);
        }
    }
}
```

Por último se tiene la función del click del botón de ejecutar que llama a los métodos anteriores.
* Primero revisa si se ha abierto un video.
* Obtiene los frames de este para utilizarlos más adelante en los cálculos de conversión a segundos.
* Prepara la carpeta donde se guardan las imágenes decodificadas.
* Decodifica la imágenes más importantes con ffmpeg.
* Llama al método secuencial y mide el tiempo de ejecución de este.
* Llama al método paralelo y mide el tiempo de ejecución de este. 

```csharp
private async void btnRun_Click(object sender, EventArgs e)
{
    if (videoPath != null)
    {
        videoFPS = GetFpsInVideo();
        PrepareImagesFolder();
        DecodeKeyImages();

        adultFrames = maleFrames = femaleFrames = totalFrames = 0;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        await ProcessImages(false);
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        lblSecuential.Text = string.Format("{0:00}:{1:00}:{2:00}.{3}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

        dgvAdult.Rows.Clear();
        dgvMaleFemale.Rows.Clear();

        adultFrames = maleFrames = femaleFrames = 0;
        stopwatch = new Stopwatch();
        stopwatch.Start();
        await ProcessImages(true);
        stopwatch.Stop();
        ts = stopwatch.Elapsed;
        lblParallel.Text = string.Format("{0:00}:{1:00}:{2:00}.{3}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
    }
}
```

# Resultados finales

![Alt Text](./images/run-example.gif)

En este caso se observa una gran diferencia entre el método paralelo y el secuecial, siendo el primero mucho más rápido. Esto es por a pesar de que el secuencial hace uso de palabras clave del lenguaje como async o await esto no es realmente paralelismo, lo que esto significa es que la ejecución de este código se hará sin bloquear el thread principal, el cual es el encargado de la parte de renderizado. En el método secuencial las peticiones son ejecutadas de esta manera sin bloquear el thread principal gracias a async y await.

![ui](images/secuential.png)

Mientras que en la versión paralela se asemeja más a esta imagen.

![ui](images/parallel.png)

# Recomendaciones

* Se puede mejorar el código para que no supere los límites de Azure, para esto cada vez que se ejecuten más de 20 transacciones en menos de un minuto se puede modificar el código para haga una espera obligatoria de un minuto antes de continuar la ejecución.
* En el método paralelo se recomienda que si son muchas imágenes en lugar de ejecutar todas la peticiones de golpe es mejor ejecutar las peticiones por batches o grupos n cantidad, esto para evitar sobrecarga por la creación de muchos threads. Tal principio se demuestra mejor en este ejemplo: https://www.michalbialecki.com/en/2018/04/19/how-to-send-many-requests-in-parallel-in-asp-net-core/
