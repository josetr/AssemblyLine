// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.Blazor.Shared;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

public partial class AdditionalErrors : IAdditionalErrors
{
    private ValidationMessageStore _messageStore = default!;

    [CascadingParameter] public EditContext CurrentEditContext { get; set; } = default!;
    public string Error { get; set; } = string.Empty;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
            throw new ArgumentNullException(@$"{nameof(AdditionalErrors)} requires a cascading parameter of type {nameof(EditContext)}.");

        _messageStore = new ValidationMessageStore(CurrentEditContext);
        CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
    }

    public void DisplayErrors(Dictionary<string, List<string>> errors)
    {
        if (_messageStore == null)
            return;

        foreach (var error in errors)
            _messageStore.Add(CurrentEditContext.Field(error.Key), error.Value);

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void DisplayError(string message)
    {
        Error = message;
        StateHasChanged();
    }

    public void DisplayError(string key, string message)
    {
        DisplayErrors(new Dictionary<string, List<string>>() { [key] = new List<string>() { message } });
    }

    public void DisplayError(Exception e)
    {
        switch (e)
        {
            case FieldTakenException violation:
                DisplayError(violation.Field, $"{violation.Field} is already taken. Please select a new one.");
                return;
            default:
                DisplayError("An unexpected error ocurred. Please try again later.");
                return;
        }
    }
}
