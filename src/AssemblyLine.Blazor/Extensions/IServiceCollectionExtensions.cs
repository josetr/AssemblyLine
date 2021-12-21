// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

using VM = AssemblyLine.ViewModels;

namespace AssemblyLine.Blazor.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<VM.Operations.IndexViewModel>();
        services.AddTransient<VM.Devices.IndexViewModel>();

        services.AddTransient<VM.Operations.AddOperationDialogViewModel>();
        services.AddTransient<VM.Operations.EditOperationDialogViewModel>();
        services.AddTransient<VM.Devices.AddDeviceDialogViewModel>();

        DialogService.Register<VM.Operations.AddOperationDialogViewModel, Pages.Operations.AddOperationDialog>();
        DialogService.Register<VM.Operations.EditOperationDialogViewModel, Pages.Operations.EditOperationDialog>();
        DialogService.Register<VM.Devices.AddDeviceDialogViewModel, Pages.Devices.AddDeviceDialog>();
    }
}
