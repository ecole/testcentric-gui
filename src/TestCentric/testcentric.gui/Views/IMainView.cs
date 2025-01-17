// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Elements;

    public interface IMainView
    {
        // View Parameters
        Point Location { get; set; }
        Size Size { get; set; }
        bool Maximized { get; set; }
        Font Font { get; set; }
        int SplitterPosition { get; set; }

        // UI Elements
        ISelection ResultTabs { get; }

        // File Menu Items
        IPopup FileMenu { get; }
        ICommand OpenCommand { get; }
        ICommand CloseCommand { get; }
        ICommand AddTestFilesCommand { get; }
        ICommand ReloadTestsCommand { get; }
        IPopup SelectAgentMenu { get; }
        IChecked RunAsX86 { get; }
        IPopup RecentFilesMenu { get; }
        ICommand ExitCommand { get; }

        // View Menu Items
        ISelection GuiLayout { get; }
        ICommand IncreaseFontCommand { get; }
        ICommand DecreaseFontCommand { get; }
        ICommand ChangeFontCommand { get; }
        ICommand RestoreFontCommand { get; }
        ICommand IncreaseFixedFontCommand { get; }
        ICommand DecreaseFixedFontCommand { get; }
        ICommand RestoreFixedFontCommand { get; }

        // Test Menu Items
        ICommand RunAllMenuCommand { get; }
        ICommand RunSelectedMenuCommand { get; }
        ICommand RunFailedMenuCommand { get; }
        ICommand StopRunMenuCommand { get; }
        ICommand ForceStopMenuCommand { get; }
        ICommand TestParametersMenuCommand { get; }

        // Tools Menu Items
        IPopup ToolsMenu { get; }
        ICommand SaveResultsCommand { get; }
        IPopup SaveResultsAsMenu { get; }
        ICommand OpenWorkDirectoryCommand { get; }
        ICommand ExtensionsCommand { get; }
        ICommand SettingsCommand { get; }

        // Help Menu Items
        ICommand TestCentricHelpCommand { get; }
        ICommand NUnitHelpCommand { get; }
        ICommand AboutCommand { get; }

        // Toolbar Items
        ICommand RunButton { get; }
        ICommand RunAllToolbarCommand { get; }
        ICommand RunSelectedToolbarCommand { get; }
        ICommand TestParametersToolbarCommand { get; }

        ICommand DebugButton { get; }
        ICommand DebugAllToolbarCommand { get; }
        ICommand DebugSelectedToolbarCommand { get; }

        ICommand StopRunButton { get; }
        ICommand ForceStopButton { get; }

        IToolTip FormatButton { get; }
        ISelection DisplayFormat { get; }
        ISelection GroupBy { get; }

        ICommand RunSummaryButton { get; }

        // SubViews
        TestTreeView TreeView { get; }
        StatusBarView StatusBarView { get; }
        ProgressBarView ProgressBarView { get; }
        TestPropertiesView TestPropertiesView { get; }
        IMessageDisplay MessageDisplay { get; }

        // Dialog Manager
        IDialogManager DialogManager { get; }

        IRunSummaryDisplay RunSummaryDisplay { get; }

        // Methods used by Presenter
        void Configure(bool useFullGui);
        void SetTitleBar(string fileName);

        // Form methods that we have to use
        void Close();

        // Form Events that we use
        event EventHandler Load;
        event EventHandler Shown;
        event EventHandler Move;
        event EventHandler Resize;
        event FormClosingEventHandler FormClosing;

        // Our own events
        event EventHandler SplitterPositionChanged;
    }
}
