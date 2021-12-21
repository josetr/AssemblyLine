// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public interface IAdditionalErrors
{
    string Error { get; set; }

    void DisplayError(Exception e);
    void DisplayError(string message);
    void DisplayError(string key, string message);
    void DisplayErrors(Dictionary<string, List<string>> errors);
}
