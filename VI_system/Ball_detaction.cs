using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI_system.Properties;



namespace VI_system
{
    public class Ball_detaction
    {

        string modelAPI = "https://visionoppgave-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/d2266057-b9f2-4d8e-a4a8-14fe78ac2edc/detect/iterations/BallModel1/image";
        string predictionEndpoint = "https://visionoppgave-prediction.cognitiveservices.azure.com/";
        string predictionKey = "33e8276cbca445e193a916f549e95b2d";
        string predictionResourceId = "/subscriptions/f417f081-089c-4c18-a54b-16aac0203a3c/resourceGroups/Hjemme-nettverk/providers/Microsoft.CognitiveServices/accounts/Visionoppgave";
        string trainingEndpoint = "https://visionoppgave.cognitiveservices.azure.com/";
        string trainingKey = "987f7ea480f54f72929f0599704562d0";


        static string publishedModelName = "BallModel8"; //Denne finner vi under performace/iteration n/publish (Vi velger navn når vi publisher)
        public string projectID = "d2266057-b9f2-4d8e-a4a8-14fe78ac2edc";
        public string[] balls = new string[20];
        public Double[] balls_x = new Double[20];
        public Double[] ball_y = new Double[20];
        public Double[] precent = new Double[20];

        public Ball_detaction()
        {
            TestIteration();
        }
        public void TestIteration()
        {
            string fileName = string.Format(@"D:\Nedlastinger\ert.png");
            System.Drawing.Image g = System.Drawing.Image.FromFile(@"D:\ert.png");
            CustomVisionTrainingClient TrainingApi = AuthenticateTraining(trainingEndpoint, trainingKey, predictionKey); Ball_detaction ball = new Ball_detaction();

            CustomVisionPredictionClient predictionApi = AuthenticatePrediction(predictionEndpoint, predictionKey);
            var pros = TrainingApi.GetProjects();

            Project pro = pros[0];
            string b = "";
            var streamm = new System.IO.MemoryStream();
            g.Save(streamm, ImageFormat.Jpeg);
            streamm.Position = 0;
            using (var stream = streamm)
            {
                int loop = 0;
                var result = predictionApi.DetectImage(pro.Id, publishedModelName, stream);

                // Loop over each prediction and write out the results

                foreach (var c in result.Predictions)
                {
                    if (c.TagName != b && c.Probability >= 0.50)
                    {
                        //Console.WriteLine($"\t{c.TagName}: {c.Probability:P1} [ {c.BoundingBox.Left}, {c.BoundingBox.Top}, {c.BoundingBox.Width}, {c.BoundingBox.Height} ]");

                        ball.balls[loop] = c.TagName;
                        ball.balls_x[loop] = c.BoundingBox.Left;
                        ball.ball_y[loop] = c.BoundingBox.Top;
                        precent[loop] = c.Probability;
                        b = c.TagName;
                        loop++;


                    }
                }

            }

           
        }

        static private CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey, string predictionKey)
        {
            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = endpoint
            };
            return trainingApi;
        }
        static private CustomVisionPredictionClient AuthenticatePrediction(string endpoint, string predictionKey)
        {
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(predictionKey))
            {
                Endpoint = endpoint
            };
            return predictionApi;
        }

    }
}
