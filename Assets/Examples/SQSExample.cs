//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the Amazon Software License (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located at
// 
//     http://aws.amazon.com/asl/
// 
// or in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//
using UnityEngine;
using System.Collections;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.SQS;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace AWSSDK.Examples
{
    public class SQSExample : MonoBehaviour
    {
        //public AlexaUnityFunctions alexaFunc;

        public PlaceObject placeObject;

        public Light[] env_light
        {
            get
            {
                return FindObjectsOfType<Light>();
            }
            set
            {
                env_light = value;
            }
        }

        Dictionary<string, Color> colorDictionay = new Dictionary<string, Color>()
        {
            { "red", Color.red }, { "blue", Color.blue }, { "green", Color.green }
        };

        //identity pool id for cognito credentials

        //changeThis
        private string IdentityPoolId = "us-east-1:ffd0d076-ce85-47ec-8232-bed8710f151f";

        public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }

        public string SQSRegion = RegionEndpoint.USEast1.SystemName;

        private RegionEndpoint _SQSRegion
        {
            get { return RegionEndpoint.GetBySystemName(SQSRegion); }
        }


        //name of the queue you want to create

        //changeThis
        private string QueueName = "aechack";

        private AWSCredentials _credentials;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
                return _credentials;
            }
        }

        private IAmazonSQS _sqsClient;

        private IAmazonSQS SqsClient
        {
            get
            {
                if (_sqsClient == null)
                    _sqsClient = new AmazonSQSClient(Credentials, _SQSRegion);
                return _sqsClient;
            }
        }

        //public Button CreateQueue;
        //public Button SendMessage;
        //public Button RetrieveMessage;
        //public Button DeleteQueue;
        //public InputField Message;

        //changeThis
        private string queueUrl = "https://sqs.us-east-1.amazonaws.com/603907316947/aechack" + "/?Action=SetQueueAttributes&Attribute.Name=ReceiveMessageWaitTimeSeconds&Attribute.Value=20";

        // Use this for initialization
        void Start()
        {
            UnityInitializer.AttachToGameObject(this.gameObject);
            //CreateQueue.onClick.AddListener(CreateQueueListener);
            //SendMessage.onClick.AddListener(SendMessageListener);
            //RetrieveMessage.onClick.AddListener(RetrieveMessageListener);
            //DeleteQueue.onClick.AddListener(DeleteQueueListener);


            StartCoroutine(RepeatRetrieveMessage(0.1F));
        }

        /*
        private void CreateQueueListener()
        {
            //            SqsClient.CreateQueueAsync(QueueName, (result) =>
            //            {
            //                if (result.Exception == null)
            //                {
            //                    Debug.Log(@"Queue Created");
            //                    queueUrl = result.Response.QueueUrl;
            //                }
            //                else
            //                {
            //                    Debug.LogException(result.Exception);
            //                }
            //            });
        }

        private void DeleteQueueListener()
        {
            //            if (!string.IsNullOrEmpty(queueUrl))
            //            {
            //                SqsClient.DeleteQueueAsync(queueUrl, (result) =>
            //                {
            //                    if (result.Exception == null)
            //                    {
            //                       Debug.Log(@"Queue Deleted");
            //                    }
            //                    else
            //                    {
            //                        Debug.LogException(result.Exception);
            //                    }
            //                });
            //            }
            //            else
            //            {
            //                Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
            //            }
        }

        private void SendMessageListener()
        {

            if (!string.IsNullOrEmpty(queueUrl))
            {
                var message = Message.text;
                if (string.IsNullOrEmpty(message))
                {
                    Debug.Log("No Message to send");
                    return;
                }

                SqsClient.SendMessageAsync(queueUrl, message, (result) =>
                {
                    if (result.Exception == null)
                    {
                        Debug.Log("Message Sent");
                    }
                    else
                    {
                        Debug.LogException(result.Exception);
                    }
                });
            }
            else
            {
                Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
            }
        }
        */

        IEnumerator RepeatRetrieveMessage(float waitTime)
        {
            bool checkSQS = true;
            while (checkSQS)
            {
                yield return new WaitForSeconds(waitTime);

                if (!string.IsNullOrEmpty(queueUrl))
                {
                    SqsClient.ReceiveMessageAsync(queueUrl, (result) =>
                    {
                        if (result.Exception == null)
                        {
                            //Read the message
                            var messages = result.Response.Messages;
                            messages.ForEach(m =>
                            {
                                //Debug.Log(@"Message Id  = " + m.MessageId);
                                Debug.Log(@"Mesage = " + m.Body);

                                //Process the message
                                //[do your thing here]

                                //var color = colorDictionay.Where(c => c.Key == m.Body).FirstOrDefault().Value;

                                if(m.Body == "white")
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].color = Color.white;

                                if (m.Body == "red")
                                {
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].color = Color.red;
                                }
                                else if (m.Body == "blue")
                                {
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].color = Color.blue;
                                }
                                else if (m.Body == "green")
                                {
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].color = Color.green;
                                }

                                if (m.Body == "off")
                                {
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].enabled = false;
                                }
                                else if (m.Body == "on")
                                    for (int i = 0; i < env_light.Length; i++)
                                        env_light[i].enabled = true;

                                if (m.Body == "delete")
                                {
                                    StartCoroutine(placeObject.ClearRoom());
                                }

                                if (m.Body == "undo")
                                {
                                    StartCoroutine(placeObject.UndoPlacement());
                                }


                                //Delete the message
                                var delRequest = new Amazon.SQS.Model.DeleteMessageRequest
                                {
                                    QueueUrl = queueUrl,
                                    ReceiptHandle = m.ReceiptHandle
                                };

                                SqsClient.DeleteMessageAsync(delRequest, (delResult) =>
                                {
                                    if (delResult.Exception == null)
                                    {
                                    }
                                    else
                                    {
                                    }
                                });
                            });

                        }
                        else
                        {
                            Debug.LogException(result.Exception);
                        }


                    });
                }
                else
                {
                    Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
                }

                //Debug.Log (".");
            }
        }


        private void RetrieveMessageListener()
        {
            StartCoroutine(RepeatRetrieveMessage(0.1F));
        }

    }

}