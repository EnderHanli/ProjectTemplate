using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using ProjectTemplate.Wizard.Properties;

namespace ProjectTemplate.Wizard
{
    public class MainWizard : IWizard
    {
        private static string _solutionName;
        private const string LibraryFolderName = "Lib";
        private _DTE _dte;
        private WizardRunKind _runKind;
        private Dictionary<string, string> _replacementsDictionary;

        public MainWizard()
        {
        }

        private string GetSolutionName()
        {
            return _replacementsDictionary["$solutionname$"];
        }

        private string GetSolutionFileName()
        {
            return GetSolutionName() + ".sln";
        }

        private string GetSolutionFileFullName()
        {
            return _dte.Solution.Properties.Item("Path").Value.ToString();
        }

        private string GetSolutionRootPath()
        {
            return GetSolutionFileFullName().Replace(GetSolutionFileName(), "");
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            if (_runKind == WizardRunKind.AsMultiProject)
            {
                string solutionPath = GetSolutionRootPath();

                string libraryFolderPath = solutionPath + "\\" + LibraryFolderName;

                string appendeskPath = solutionPath + "\\" + LibraryFolderName + "\\Appendesk.dll";

                if (Directory.Exists(libraryFolderPath) == false)
                {
                    Directory.CreateDirectory(libraryFolderPath);
                }

                File.WriteAllBytes(appendeskPath, Properties.Resources.Appendesk);
            }
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            this._dte = automationObject as _DTE;
            this._runKind = runKind;
            this._replacementsDictionary = replacementsDictionary;

            if (runKind == WizardRunKind.AsMultiProject)
            {
                _solutionName = replacementsDictionary["$safeprojectname$"];
            }

            replacementsDictionary.Add("$solutionname$", _solutionName);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}