﻿using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using Creationline.Mirage.Application.Common.Interfaces;
using Creationline.Mirage.Application.Common.Models;
using Creationline.Mirage.Application.TodoLists.Queries;
using Creationline.Mirage.Domain.Todo.Entities;
using Creationline.Mirage.Application.TodoItems.Queries;

namespace CleanArchitecture.Application.UnitTests.Common.Mappings;

public class マッピング機能
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public マッピング機能()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void AutoMapperのマッピングルールや設定が適切に定義されている()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(TodoList), typeof(TodoListDto))]
    [InlineData(typeof(TodoItem), typeof(TodoItemDto))]
    [InlineData(typeof(TodoList), typeof(LookupDto))]
    [InlineData(typeof(TodoItem), typeof(LookupDto))]
    [InlineData(typeof(TodoItem), typeof(TodoItemBriefDto))]
    public void AutoMapperのマッピングが適切に設定されている(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        // TODO: Figure out an alternative approach to the now obsolete `FormatterServices.GetUninitializedObject` method.
#pragma warning disable SYSLIB0050 // Type or member is obsolete
        return FormatterServices.GetUninitializedObject(type);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
    }
}
