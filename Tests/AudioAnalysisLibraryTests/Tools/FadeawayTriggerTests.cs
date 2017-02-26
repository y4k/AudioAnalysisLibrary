using System;
using AudioAnalysisLibrary.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AudioAnalysisLibraryTests.Tools
{
	[TestClass]
    public class FadeawayTriggerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FadeawayTrigger_StopValueHigherThanStartValueThrowsException()
        {
            var fadeawayTrigger = new FadeawayTrigger<int>(10,11);
        }

        [TestMethod]
        public void FadeawayTrigger_OnStart_IsRunningTrue()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            Assert.IsTrue(fade.IsRunning);
        }

        [TestMethod]
        public void FadeawayTrigger_OnStartAndStop_IsRunningTrueThenFalse()
        {
            var fade = new FadeawayTrigger<int>(11, 10);

            fade.Start();
            Assert.IsTrue(fade.IsRunning);

            fade.Stop();
            Assert.IsFalse(fade.IsRunning);
        }

        [TestMethod]
        public void FadeawayTrigger_Running_UpdateAlwaysReturnsTrue()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            for (var i = 0; i < 15; i++)
            {
                var actual = fade.Update(i);
                Assert.IsTrue(actual);
            }
        }

        [TestMethod]
        public void FadeawayTrigger_NotRunning_UpdateAllReturnsFalse()
        {
            var fade = new FadeawayTrigger<int>(11, 10);

            for (var i = 0; i < 15; i++)
            {
                var actual = fade.Update(i);
                Assert.IsFalse(actual);
            }
        }

        [TestMethod]
        public void FadeawayTrigger_RunningButInactive_UpdateEqualToStartValueRaisesEvent()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            var actual = false;

            fade.StateChange += status =>
            {
                actual = status;
            };

            fade.Update(12);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void FadeawayTrigger_RunningButInactive_UpdateAboveStartValueRaisesEvent()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            var actual = false;

            fade.StateChange += status =>
            {
                actual = status;
            };

            fade.Update(13);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void FadeawayTrigger_RunningButInactive_UpdateBelowStartEventDoesNotRaiseEvent()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();
            
            var actual = false;

            fade.StateChange += status =>
            {
                actual = status;
            };

            fade.Update(10);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void FadeawayTrigger_Running_MultipleUpdateAboveStartRaisesOnlyOneEvent()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            var actual = false;

            fade.StateChange += status =>
            {
                actual = status;
            };

            fade.Update(12);

            Assert.IsTrue(actual);
            
            actual = false;

            fade.Update(12);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void FadeawayTrigger_Running_UpdateEqualToStopValueTriggersOnlyIfActive()
        {
            var fade = new FadeawayTrigger<int>(11, 10);
            fade.Start();

            var actual = false;

            fade.StateChange += status =>
            {
                actual = status;
            };

            //Not active
            fade.Update(10);
            Assert.IsFalse(actual);

            //Activate with value equal to start value.
            fade.Update(11);
            Assert.IsTrue(actual);

            //Deactivate with value equal to stop value
            fade.Update(10);
            Assert.IsTrue(actual);

            //Set actual to true and update with stop value...
            actual = true;
            fade.Update(10);
            //...Should not change actual
            Assert.IsTrue(actual);
        }
    }
}