using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;

namespace CognitiveServicesSO
{
    public class VisionAPI
    {
        private const string SUBSCRIPTION_KEY = Keys.VISION_KEY;
        private const string ENDPOINT = Keys.VISION_ENDPOINT;
        private ComputerVisionClient computerVision;
        private List<VisualFeatureTypes?> features;

        public VisionAPI(List<VisualFeatureTypes?> features)
        {
            computerVision =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(SUBSCRIPTION_KEY))
                { Endpoint = ENDPOINT };
            this.features = features;
        }

        public Task<ImageAnalysis> StartRecognizing(Stream image)
        {
            return computerVision.AnalyzeImageInStreamAsync(image, features);
        }
    }
}
