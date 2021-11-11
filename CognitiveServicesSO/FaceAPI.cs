using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServicesSO
{
    public class FaceAPI
    {
        private const string SUBSCRIPTION_KEY = Keys.FACE_KEY;
        private const string ENDPOINT = Keys.FACE_ENDPOINT;
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
    }
}
