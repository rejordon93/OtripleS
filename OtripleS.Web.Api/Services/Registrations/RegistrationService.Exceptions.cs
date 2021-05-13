﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;

namespace OtripleS.Web.Api.Services.Registrations
{
    public partial class RegistrationService
    {
        private delegate ValueTask<Registration> ReturningRegistrationFunction();

        private async ValueTask<Registration> TryCatch(ReturningRegistrationFunction returningRegistrationFunction)
        {
            try
            {
                return await returningRegistrationFunction();
            }
            catch (InvalidRegistrationException invalidRegistrationInputException)
            {
                throw CreateAndLogValidationException(invalidRegistrationInputException);
            }
            catch (NotFoundRegistrationException notFoundRegistrationException)
            {
                throw CreateAndLogValidationException(notFoundRegistrationException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedRegistrationException = new LockedRegistrationException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedRegistrationException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private RegistrationValidationException CreateAndLogValidationException(Exception exception)
        {
            var RegistrationValidationException = new RegistrationValidationException(exception);
            this.loggingBroker.LogError(RegistrationValidationException);

            return RegistrationValidationException;
        }

        private RegistrationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var registrationDependencyException = new RegistrationDependencyException(exception);
            this.loggingBroker.LogCritical(registrationDependencyException);

            return registrationDependencyException;
        }

        private RegistrationDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var registrationDependencyException = new RegistrationDependencyException(exception);
            this.loggingBroker.LogError(registrationDependencyException);

            return registrationDependencyException;
        }

        private RegistrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var registrationServiceException = new RegistrationServiceException(exception);
            this.loggingBroker.LogError(registrationServiceException);

            return registrationServiceException;
        }
    }
}
