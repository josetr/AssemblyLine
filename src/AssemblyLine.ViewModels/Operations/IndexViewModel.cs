// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels.Operations;

public class IndexViewModel : ViewModelBase
{
    public IndexViewModel(IOperationService operationService, IDialogService dialogService, ISnackbar snackbar)
    {
        OperationService = operationService;
        DialogService = dialogService;
        Snackbar = snackbar;
    }

    public IOperationService OperationService;
    public IDialogService DialogService;
    public ISnackbar Snackbar;
    public List<Operation> Operations { get; set; } = new();

    public override async Task OnInitializedAsync()
    {
        await LoadOperationsAsync();
    }

    public async Task OpenCreateOperationDialogAsync()
    {
        var operation = await DialogService.Show<AddOperationDialogViewModel, Operation>("Add Operation");
        if (operation == null)
            return;

        Operations.Add(operation);
        Snackbar.Show($"Added operation {operation.Name}", Severity.Success);
    }

    public async Task OpenEditOperationDialogAsync(Operation operation)
    {
        var editedOperation = await DialogService.Show<EditOperationDialogViewModel, Operation, int>("Edit Operation", nameof(EditOperationDialogViewModel.OperationId), operation.Id);
        if (editedOperation == null)
            return;

        Operations.Replace(editedOperation, x => x.Id);
        Snackbar.Show("Updated operation", Severity.Success);
    }

    public async Task MoveOperationDownAsync(Operation operation)
    {
        if (await OperationService.ChangeOrderAsync(operation, 1))
            await LoadOperationsAsync();
    }

    public async Task MoveOperationUpAsync(Operation operation)
    {
        if (await OperationService.ChangeOrderAsync(operation, -1))
            await LoadOperationsAsync();
    }

    public async Task DeleteOperationAsync(Operation operation)
    {
        if (await DialogService.ShowMessageBox("Delete", $"Do you really want to delete the operation '{operation.Name}'?", yesText: "yes", noText: "no") != true)
            return;

        await OperationService.DeleteOperationAsync(operation.Id);
        Operations.Remove(operation);
        Snackbar.Show($"The operation '{operation.Name}' has been deleted.", Severity.Success);
    }

    private async Task LoadOperationsAsync()
    {
        Operations = (await OperationService.GetOperationsAsync()).ToList();
    }
}
