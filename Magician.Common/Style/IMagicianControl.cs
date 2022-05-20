using System;
namespace Magician.Common.Style
{
    public interface IMagicianControl
    {
        BaseStyle.EPresetStyle PresetStyle { get; set; }
    }
}