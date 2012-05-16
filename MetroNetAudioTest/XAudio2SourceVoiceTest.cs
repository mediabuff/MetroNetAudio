﻿namespace MetroNetAudioTest
{
    using System.Diagnostics;
    using System.Threading;
    using MetroNetAudio;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class XAudio2SourceVoiceTest
    {
        [TestMethod]
        public void CanGetState()
        {
            XAudio2 obj = XAudio2.Create();
            var master = obj.CreateMasteringVoice();
            var voice = obj.CreateSourceVoice();
            var state = voice.GetState(VoiceStateFlags.None);

            Assert.IsNotNull(state);
            Assert.AreEqual(0U, state.BuffersQueued);
            Assert.AreEqual(0U, state.SamplesPlayed);
        }

        [TestMethod]
        public void CanSubmitBuffer()
        {
            XAudio2 obj = XAudio2.Create();
            var master = obj.CreateMasteringVoice();
            var voice = obj.CreateSourceVoice();
            voice.SubmitSourceBuffer(new XAudio2Buffer());

            var state = voice.GetState(VoiceStateFlags.None);

            Assert.IsNotNull(state);
            Assert.AreEqual(1U, state.BuffersQueued);
            Assert.AreEqual(0U, state.SamplesPlayed);
        }

        [TestMethod]
        public void CanStartVoice()
        {
            XAudio2 obj = XAudio2.Create();
            var master = obj.CreateMasteringVoice();
            var voice = obj.CreateSourceVoice();

            voice.Start();
        }

        [TestMethod]
        public void CanStopVoice()
        {
            XAudio2 obj = XAudio2.Create();
            var master = obj.CreateMasteringVoice();
            var voice = obj.CreateSourceVoice();

            voice.Stop();
        }

        [TestMethod]
        public void StartingVoiceFiresVoiceProcessingPassStart()
        {
            XAudio2 obj = XAudio2.Create();
            var master = obj.CreateMasteringVoice();
            var voice = obj.CreateSourceVoice();

            uint bytesRequested = 0;
            var evt = new ManualResetEvent(false);

            voice.VoiceProcessingPassStart += (bytes) =>
            {
                bytesRequested = bytes;
                evt.Set();
            };
            
            voice.Start();
            evt.WaitOne(10000); // allow 10 secs for the event to occur    
            voice.Stop();

            Assert.AreNotEqual(0, bytesRequested);
        }
    }
}
