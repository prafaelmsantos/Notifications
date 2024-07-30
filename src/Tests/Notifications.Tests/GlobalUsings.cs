﻿global using System.Linq.Expressions;
global using AutoMapper;
global using Bogus;
global using FluentAssertions;
global using Microsoft.EntityFrameworkCore.Query;
global using Moq;
global using Notifications.Core;
global using Notifications.Core.Domain;
global using Notifications.Core.Domain.Enum;
global using Notifications.Core.DTO;
global using Notifications.Core.DTO.Internal.Response;
global using Notifications.Core.DTO.Visitor;
global using Notifications.MessageProcessor.Configs;
global using Notifications.MessageProcessor.Interfaces;
global using Notifications.Persistence.Interfaces.Repositories;
global using Notifications.Persistence.Interfaces.Services;
global using Notifications.Persistence.Services;
global using Notifications.Tests.Common;
global using Notifications.Tests.Core.Builders;
global using Xunit;
global using Xunit.Abstractions;
