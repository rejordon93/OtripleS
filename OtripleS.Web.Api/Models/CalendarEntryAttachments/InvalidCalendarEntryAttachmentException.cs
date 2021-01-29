﻿//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments
{
    public class InvalidCalendarEntryAttachmentException : Exception
    {
        public InvalidCalendarEntryAttachmentException(string parameterName, object parameterValue)
            : base($"Invalid CalendarEntryAttachment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
