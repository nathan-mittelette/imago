﻿namespace imago;

public partial class App : Application
{
    private readonly MainPage _mainPage;

    public App(MainPage mainPage)
    {
        _mainPage = mainPage;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(this._mainPage);
    }
}