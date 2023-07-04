using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace ARudzbenik.General
{
    public class RemoteDataFetcher : SingletonBehaviour<RemoteDataFetcher>
    {
        private const string _SERVER_IP_ADDRESS = "http://192.168.1.162:8000"; // CHANGE TO LOCAL SERVER IP

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void FetchFileNames(string regexPattern, int matchGroup, Action<bool, string[]> onFileNamesFetched)
        {
            static IEnumerator FetchRoutine(string regexPattern, int matchGroup, Action<bool, string[]> onFetched)
            {
                UnityWebRequest request = UnityWebRequest.Get(_SERVER_IP_ADDRESS);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Fetch failed! Response code: " + request.responseCode);
                    onFetched?.Invoke(false, null);
                    yield break;
                }

                List<string> fileNames = new List<string>();
                Regex regex = new Regex(regexPattern);
                foreach (Match match in regex.Matches(request.downloadHandler.text).Cast<Match>())
                {
                    if (match.Groups[matchGroup].Value.Length > 0) fileNames.Add(match.Groups[matchGroup].Value);
                }
                onFetched?.Invoke(true, fileNames.ToArray());
            }
            StartCoroutine(FetchRoutine(regexPattern, matchGroup, onFileNamesFetched));
        }

        public void FetchJSONFile(string filename, Action<bool, string> onJSONFileFetched)
        {
            static IEnumerator FetchCoroutine(string filename, Action<bool, string> onFetched)
            {
                UnityWebRequest request = UnityWebRequest.Get(_SERVER_IP_ADDRESS + "/" + filename + ".json");
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Fetch failed! Response code: " + request.responseCode);
                    onFetched?.Invoke(false, null);
                }
                else onFetched?.Invoke(true, request.downloadHandler.text);
            }
            StartCoroutine(FetchCoroutine(filename, onJSONFileFetched));
        }

        public void FetchTexture(string filename, Action<bool, Texture> onTextureFetched)
        {
            static IEnumerator FetchCoroutine(string filename, Action<bool, Texture> onFetched)
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(_SERVER_IP_ADDRESS + "/" + filename);
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Fetch failed! Response code: " + request.responseCode);
                    onFetched?.Invoke(false, null);
                }
                else
                {
                    DownloadHandlerTexture handler = request.downloadHandler as DownloadHandlerTexture;
                    onFetched?.Invoke(true, handler.texture);
                }    
            }
            StartCoroutine(FetchCoroutine(filename, onTextureFetched));
        }
    }
}