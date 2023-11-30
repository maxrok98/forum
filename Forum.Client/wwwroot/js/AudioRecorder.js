﻿var BlazorAudioRecorder = {};

(function () {
    var mStream;
    var mAudioChunks;
    var mMediaRecorder;
    var mCaller;
    var mRecorder;

    BlazorAudioRecorder.Initialize = function (vCaller) {
        mCaller = vCaller;
    };

    BlazorAudioRecorder.StartRecord = async function () {
      //mStream = await navigator.mediaDevices.getUserMedia({ audio: true }, function (stream) {
      //  let context = new AudioContext();
      //  let mediaStreamSource = context.createMediaStreamSource(stream);
      //  mRecorder = new Recorder(mediaStreamSource);
      //  mRecorder.record();
      //},
      //function (err) {
      //  console.log('The following error occurred: ' + err);
      //});
      mStream = await navigator.mediaDevices.getUserMedia({ audio: true }).then(
        function (stream) {
          let context = new AudioContext();
          let mediaStreamSource = context.createMediaStreamSource(stream);
          mRecorder = new Recorder(mediaStreamSource);
          mRecorder.record();
        },
        function (err) {
          console.log("Could not find recording device - " + err);
        });
          //mMediaRecorder = new MediaRecorder(mStream);

          //mMediaRecorder.addEventListener('dataavailable', vEvent => {
          //    mAudioChunks.push(vEvent.data);
          //});

          //mMediaRecorder.addEventListener('error', vError => {
          //    console.warn('media recorder error: ' + vError);
          //});

          //mMediaRecorder.addEventListener('stop', async () => {
          //    var pAudioBlob = new Blob(mAudioChunks, { type: "audio/wav;" });
          //    const content = await (new Response(pAudioBlob).arrayBuffer());
          //    var pAudioUrl = URL.createObjectURL(pAudioBlob);
          //    mCaller.invokeMethodAsync('OnAudioReady', new Uint8Array(content), pAudioUrl);

          //    // uncomment the following if you want to play the recorded audio (without the using the audio HTML element)
          //    //var pAudio = new Audio(pAudioUrl);
          //    //pAudio.play();
          //});

          //mAudioChunks = [];
          //mMediaRecorder.start();
    };

    BlazorAudioRecorder.PauseRecord = function () {
        mMediaRecorder.pause();
    };

    BlazorAudioRecorder.ResumeRecord = function () {
        mMediaRecorder.resume();
    };

    BlazorAudioRecorder.StopRecord = function () {
        //mMediaRecorder.stop();
      //mStream.getTracks().forEach(pTrack => pTrack.stop());
      mRecorder.stop();

      mRecorder.exportWAV(async function (pAudioBlob) {
            const content = await (new Response(pAudioBlob).arrayBuffer());
            var pAudioUrl = URL.createObjectURL(pAudioBlob);
            mCaller.invokeMethodAsync('OnAudioReady', new Uint8Array(content), pAudioUrl);
        });
    };

    BlazorAudioRecorder.DownloadBlob = function (vUrl, vName) {
        // Create a link element
        const link = document.createElement("a");

        // Set the link's href to point to the Blob URL
        link.href = vUrl;
        link.download = vName;

        // Append link to the body
        document.body.appendChild(link);

        // Dispatch click event on the link
        // This is necessary as link.click() does not work on the latest firefox
        link.dispatchEvent(
            new MouseEvent('click', {
                bubbles: true,
                cancelable: true,
                view: window
            })
        );

        // Remove the link from the body
        document.body.removeChild(link);
    };
})();
