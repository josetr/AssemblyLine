﻿// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public interface ISnackbar
{
    void Show(string msg, Severity severity);
}
