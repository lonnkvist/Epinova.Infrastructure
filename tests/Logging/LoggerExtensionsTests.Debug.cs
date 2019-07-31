﻿using System;
using System.Linq;
using EPiServer.Logging;
using Xunit;
using LoggerExtensions = Epinova.Infrastructure.Logging.LoggerExtensions;

namespace Epinova.InfrastructureTests.Logging
{
    partial class LoggerExtensionsTests
    {
        [Fact]
        public void Debug_LazyLogAnonymousObjectAndExceptionOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Debug_LazyLogAnonymousObjectAndExceptionOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, new Exception("OMG!"), (number, ex) =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Debug_LazyLogAnonymousObjectOnDisabledLevel_DoesNotCallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Error, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.False(isInvoked);
        }

        [Fact]
        public void Debug_LazyLogAnonymousObjectOnEnabledLevel_CallMessageFormatter()
        {
            var isInvoked = false;
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number =>
            {
                isInvoked = true;
                return new { message = "Hello", number };
            });
            Assert.True(isInvoked);
        }

        [Fact]
        public void Debug_LazyLogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Debug, _output);
            int state = Factory.GetInteger();

            LoggerExtensions.Debug(logger, state, number => new { message = "Hello", number });
            Assert.Equal($"DEBUG: {{\"message\":\"Hello\",\"number\":{state}}}", logger.Messages.First());
        }

        [Fact]
        public void Debug_LogAnonymousObjectOnEnabledLevel_LogsMessage()
        {
            var logger = new TestableLogger(Level.Debug, _output);
            int number = Factory.GetInteger();

            LoggerExtensions.Debug(logger, new { message = "Hello", number });
            Assert.Equal($"DEBUG: {{\"message\":\"Hello\",\"number\":{number}}}", logger.Messages.First());
        }
    }
}