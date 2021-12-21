// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public interface IDialogService
{
    Task<TResult?> Show<TViewModel, TResult, TParameter>(string name, string parameterName, TParameter? parameter);
    Task<TResult?> Show<TViewModel, TResult>(string name);
    Task<bool?> ShowMessageBox(string title, string message, string yesText, string noText);
}
