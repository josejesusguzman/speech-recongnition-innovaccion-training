//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace HelloWorld
{

    class Program
    {
        // Aquí inicias una tarea asincrona que no corre en el hilo principal y no bloquea el main
        static async Task Main()
        {
            await RecognizeSpeechAsync();
        }

        static async Task RecognizeSpeechAsync()
        {
            // Configuración de la información del recurso para acceder a el.
            // Usa la key1 o la key2 del recurso Speech Service que has creado
            var config = SpeechConfig.FromSubscription("TU_KEY_DE_COGNITIVE_SERVICE", "LOCALIZACION_DEL_RECURSO");

            // Carga el archivo de audio quq usarás. En esta ocasión desde un archivo local
            using (var audioInput = AudioConfig.FromWavFileInput("NOMBRE_ARCHIVO.wav"))

            // Aquí le pasas en parametros que necesita el Speech Service junto con el archivo audio
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
                Console.WriteLine("Recognizing first result...");
                var result = await recognizer.RecognizeOnceAsync();
                switch (result.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        // Si cae aquí, el archivo de audio fue reconocido y verás la transcripción en la terminal
                        Console.WriteLine($"We recognized: {result.Text}");
                        break;
                    case ResultReason.NoMatch:
                        // Si cae aquí, el archivo de audio NO fue reconocido por Azure y verás un mensaje de ello
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        break;
                    case ResultReason.Canceled:
                        // Si cae aquí, la operación de canceló y en terminal te manda la razón
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");
                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                        break;
                }
            }
        }
    }
}