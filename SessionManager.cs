using System.Collections.Generic;
using System.Windows;

namespace Beauty_Salon
{
    public class SessionManager
    {
        private static readonly SessionManager _instance = new SessionManager();
        private Stack<Window> windowStack = new Stack<Window>();
        private Window _initialWindow;

        public static SessionManager Instance => _instance;

        private SessionManager() 
        {
           
        }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public bool History { get; set; }
        public int ServiceID { get; set; }
        public int SpecialistID { get; set; }

        public void SetInitialWindow(Window window)
        {
            if (_initialWindow == null) 
            {
                _initialWindow = window;
                windowStack.Push(window);
            }
        }
        public void OpenWindow(Window newWindow)
        {
            windowStack.Push(newWindow);
            newWindow.Show();

            if (windowStack.Count > 1)
            {
                Window previousWindow = windowStack.ElementAt(1);
                previousWindow.Close();
            }
        }

        public void CloseCurrentWindow()
        {
            if (windowStack.Count > 0)
            {
                Window currentWindow = windowStack.Pop();
                currentWindow.Close();
            }
        }

        public void MainWindowOush()
        {
            MainWindow main = new MainWindow();
            windowStack.Push(main);
        }
    }
}
