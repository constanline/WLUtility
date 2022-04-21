using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Magician.Common.Util;

namespace Magician.Common.Plugin
{
    /// <summary>
    ///     PluginManager 用于管理所有的插件。
    /// </summary>
    public class PluginManager<inT>
    {
        private readonly IDictionary<string, IPlugin<inT>> _dicPlugins = new Dictionary<string, IPlugin<inT>>();

        public PluginManager()
        {
            PluginsChanged += delegate { };
        }

        public IList<IPlugin<inT>> PluginList => _dicPlugins.Values.ToList();

        public bool CopyToMemory { get; set; } = false;

        public event Action PluginsChanged;

        public void LoadAllPlugins(string pluginFolderPath, bool searchChildFolder)
        {
            var list = ReflectionUtil.LoadDerivedType(typeof(IPlugin<inT>), pluginFolderPath, searchChildFolder,
                CopyToMemory);
            foreach (var type in list)
            {
                var plugin = (IPlugin<inT>) Activator.CreateInstance(type);
                _dicPlugins.Add(plugin.PluginKey, plugin);
                plugin.OnLoading();
            }

            PluginsChanged?.Invoke();
        }

        public void LoadDefault()
        {
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, true);
        }

        public void LoadPluginAssembly(string assemblyPath)
        {
            var flag = CopyToMemory;
            Assembly asm;
            if (flag)
            {
                var rawAssembly = FileUtil.Read(assemblyPath);
                asm = Assembly.Load(rawAssembly);
            }
            else
            {
                asm = Assembly.LoadFrom(assemblyPath);
            }

            var list = ReflectionUtil.LoadDerivedInstance<IPlugin<inT>>(asm);
            foreach (var plugin in list)
            {
                _dicPlugins.Add(plugin.PluginKey, plugin);
                plugin.OnLoading();
            }

            PluginsChanged?.Invoke();
        }

        public void Clear()
        {
            foreach (var plugin in _dicPlugins.Values)
                try
                {
                    plugin.BeforeTerminating();
                }
                catch
                {
                    // ignored
                }

            _dicPlugins.Clear();
            PluginsChanged?.Invoke();
        }

        public void DynRemovePlugin(string pluginKey)
        {
            var plugin = GetPlugin(pluginKey);
            if (plugin == null) return;
            plugin.BeforeTerminating();
            _dicPlugins.Remove(pluginKey);
            PluginsChanged?.Invoke();
        }

        private bool ContainsPlugin(string pluginKey)
        {
            return _dicPlugins.ContainsKey(pluginKey);
        }

        public IPlugin<inT> GetPlugin(string pluginKey)
        {
            var result = ContainsPlugin(pluginKey) ? null : _dicPlugins[pluginKey];
            return result;
        }

        public void EnablePlugin(string pluginKey)
        {
            var plugin = GetPlugin(pluginKey);
            if (plugin != null) plugin.Enabled = true;
        }

        public void DisablePlugin(string pluginKey)
        {
            var plugin = GetPlugin(pluginKey);
            if (plugin != null) plugin.Enabled = false;
        }
    }
}