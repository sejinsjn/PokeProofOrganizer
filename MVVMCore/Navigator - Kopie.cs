using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace BaseWPFCollection
{
    public class Navigator
    {
        private static Navigator instance = new Navigator();

        private const string VIEW_MODEL = "ViewModel";

        private bool enableUIExtensibility = true;

        private IViewModelCreator viewModelCreator = new DefaultViewModelCreator();
        private IUIExtensibility uiAddInManager = null;

        private int currentLevel = 0;
        private string currentCaption = string.Empty;
        private List<int> currentNavigationIndicesOfLevels = new List<int>();
        public event EventHandler Navigated;
        public event EventHandler LastNavigation;

        public List<NavigationNode> RootNavigationNodeList { get; set; } = new List<NavigationNode>();

        private Navigator()
        {
        }
        
        public static Navigator Instance
        {
            get
            {
                return instance;
            }
        }

        public IViewModelCreator ViewModelCreator
        {
            set
            {
                viewModelCreator = value;
            }
        }
        

        public IUIExtensibility UiAddInManager
        {
            get
            {
                return uiAddInManager;
            }

            set
            {
                uiAddInManager = value;
            }
        }

        public bool EnableUIExtensibility
        {
            set
            {
                enableUIExtensibility = value;
            }
        }

        public int CurrentLevel
        {
            set
            {
                currentLevel = value;
            }
        }
       

        public string NaviagtionPath
        {
            get
            {
                if (rootNavigationNode.NavigationService.Content != null)
                {
                    return rootNavigationNode.NavigationService.Content.GetType().Name;
                }
                return string.Empty;
            }
        }

        public List<int> CurrentNavigationIndicesOfLevels
        {
            get
            {
                return currentNavigationIndicesOfLevels;
            }

            set
            {
                currentNavigationIndicesOfLevels = value;
            }
        }

        public void NavigateToWindow(Type typeOfWindow, string assemblyName)
        {
            NavigateToWindow(typeOfWindow, null, assemblyName);
        }

        public void NavigateToWindow(Type typeOfWindow, object data, string assemblyName)
        {
            try
            {
                object window = null;

                string typeName = typeOfWindow.FullName;
                if (typeName.StartsWith(assemblyName))
                {
                    typeName = typeName.Substring(assemblyName.Length + 1);
                }

                if (enableUIExtensibility)
                {
                    window = uiAddInManager.GetView(typeName);
                }
                if (window == null)
                {
                    window = Activator.CreateInstance(typeOfWindow);
                }

                object viewModel = null;
                if (enableUIExtensibility)
                {
                    viewModel = uiAddInManager.GetViewModel(typeName + VIEW_MODEL, viewModelCreator);
                }
                if (viewModel == null)
                {
                    Type type = Type.GetType(typeName + VIEW_MODEL + ", " + assemblyName);
                    if (type == null)
                    {
                        type = FindType(assemblyName + "." + typeName + VIEW_MODEL);
                    }

                    viewModel = viewModelCreator.Create(type);
                }

                    if (window is Window)
                {
                    if (viewModel is ViewModelBase)
                    {
                        (viewModel as ViewModelBase).Initialize(data);
                        (window as Window).DataContext = viewModel;

                    }

                    if (window is INavigatable)
                    {
                        foreach (NavigationService navigationService in (window as INavigatable).NavigationServiceContentList)
                        {
                            navigationService.Navigated -= navigationService_Navigated;
                            navigationService.Navigated += navigationService_Navigated;
                            rootNavigationNode.GetNavigationNode(currentLevel, currentNavigationIndicesOfLevels).NavigationService = navigationService;
                            rootNavigationNode.GetNavigationNode(currentLevel, currentNavigationIndicesOfLevels).SubNavigationNodeList =
                                new List<NavigationNode>();
                        }
                    }

                    (window as Window).Show();
                    (window as Window).Activate();

                    if (viewModel is ViewModelBase)
                    {
                        (viewModel as ViewModelBase).OnAfterActivated();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Navigate2(INavigatable page)
        { 
            if (page is INavigatable)
            {
                List<NavigationNode> subNavigationNodeList
                    = (page as INavigatable).NavigationServiceContentList.Select(x => new NavigationNode(currentLevel + 1, x)).ToList();

                foreach (NavigationNode navigationNode in subNavigationNodeList)
                {
                    navigationNode.NavigationService.Navigated -= navigationService_Navigated;
                    navigationNode.NavigationService.Navigated += navigationService_Navigated;
                }
                rootNavigationNode.GetNavigationNode(currentLevel, currentNavigationIndicesOfLevels).SubNavigationNodeList =
                    subNavigationNodeList;
            }
        }

    public void Refresh()
        {
            GetNavigationService().Refresh();
        }

        private NavigationService GetNavigationService()
        {
            return rootNavigationNode.GetNavigationService(currentLevel, currentNavigationIndicesOfLevels);
        }

        public void GoBack()
        {
            NavigationService navigationService =
                rootNavigationNode.GetNavigationService(currentLevel, currentNavigationIndicesOfLevels);
            if (navigationService != null)
            {
                if (navigationService.CanGoBack)
                {
                    navigationService.GoBack();
                }
            }
        }

        private Type FindType(string fullName)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName.Equals(fullName));
        }

        public void NavigateToPage(string typeNameOfPage, object data, string assemblyName)
        {
            try
            {
                object page = null;
                if (enableUIExtensibility)
                {
                    page = uiAddInManager.GetView(typeNameOfPage);
                }
                if (page == null)
                {
                    Type type = Type.GetType(typeNameOfPage + ", " + assemblyName);
                    if (type == null)
                    {
                        type = FindType(typeNameOfPage);
                    }
                    page = Activator.CreateInstance(FindType(typeNameOfPage));
                }

                object viewModel = null;
                if (enableUIExtensibility)
                {
                    viewModel = uiAddInManager.GetViewModel(typeNameOfPage + VIEW_MODEL, viewModelCreator);
                }
                if (viewModel == null)
                {
                    Type pageType = Type.GetType(typeNameOfPage + VIEW_MODEL + ", " + assemblyName);
                    if (pageType != null)
                    {
                        viewModel = viewModelCreator.Create(pageType);
                    }
                }

                if (page is Page && rootNavigationNode != null)
                {
                    NavigationService navigationService = GetNavigationService();

                    if ((navigationService != null) && (navigationService.Content is Page))
                    {
                        if ((navigationService.Content as Page).DataContext is ViewModelBase)
                        {
                            ((navigationService.Content as Page).DataContext as ViewModelBase).OnBeforeClose();
                        }
                    }

                    if (page is INavigatable)
                    {
                        List<NavigationNode> subNavigationNodeList
                            = (page as INavigatable).NavigationServiceContentList.Select(x => new NavigationNode(currentLevel + 1, x)).ToList();

                        foreach (NavigationNode navigationNode in subNavigationNodeList)
                        {
                            navigationNode.NavigationService.Navigated -= navigationService_Navigated;
                            navigationNode.NavigationService.Navigated += navigationService_Navigated;
                        }
                        rootNavigationNode.GetNavigationNode(currentLevel, currentNavigationIndicesOfLevels).SubNavigationNodeList =
                            subNavigationNodeList;
                    }
                    if (viewModel is ViewModelBase)
                    {
                        (viewModel as ViewModelBase).Initialize(data);
                        (page as Page).DataContext = viewModel;

                    }

                    if (navigationService != null)
                    {
                        navigationService.Navigate(page);
                    }

                    if (viewModel is ViewModelBase)
                    {
                        (viewModel as ViewModelBase).OnAfterActivated();
                    }

                    if (LastNavigation != null)
                    {
                        LastNavigation(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        
        public void NavigateToPage(Type typeOfPage, object data, string assemblyName)
        {
            
            NavigateToPage(typeOfPage.ToString(), data, assemblyName);
        }

        private void navigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (Navigated != null)
            {
                Navigated(null, EventArgs.Empty);
            }
            
        }
    }
}
