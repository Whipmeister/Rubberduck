﻿using System.Runtime.InteropServices;
using NLog;

namespace Rubberduck.UI.Command
{
    /// <summary>
    /// A command that displays the To-Do explorer window.
    /// </summary>
    [ComVisible(false)]
    public class ToDoExplorerCommand : CommandBase
    {
        private readonly IPresenter _presenter;

        public ToDoExplorerCommand(IPresenter presenter) : base(LogManager.GetCurrentClassLogger())
        {
            _presenter = presenter;
        }

        protected override void ExecuteImpl(object parameter)
        {
            _presenter.Show();
        }
    }
}
