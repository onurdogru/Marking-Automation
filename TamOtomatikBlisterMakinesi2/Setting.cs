using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TamOtomatikBlisterMakinesi2
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]

	internal sealed class Setting : ApplicationSettingsBase
	{
		private static Setting defaultInstance = (Setting)SettingsBase.Synchronized((SettingsBase)new Setting());

		public static Setting Default
		{
			get
			{
				return Setting.defaultInstance;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string heatingTime
		{
			get
			{
				return (string)this[nameof(heatingTime)];
			}
			set
			{
				this[nameof(heatingTime)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string vacuumTime
		{
			get
			{
				return (string)this[nameof(vacuumTime)];
			}
			set
			{
				this[nameof(vacuumTime)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string vacuumAfter
		{
			get
			{
				return (string)this[nameof(vacuumAfter)];
			}
			set
			{
				this[nameof(vacuumAfter)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string coolingTime
		{
			get
			{
				return (string)this[nameof(coolingTime)];
			}
			set
			{
				this[nameof(coolingTime)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public Int16 modelCmbBxIndex
		{
			get
			{
				return (Int16)this[nameof(modelCmbBxIndex)];
			}
			set
			{
				this[nameof(modelCmbBxIndex)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string txtPrdOrder
		{
			get
			{
				return (string)this[nameof(txtPrdOrder)];
			}
			set
			{
				this[nameof(txtPrdOrder)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string txtUProductionModel
		{
			get
			{
				return (string)this[nameof(txtUProductionModel)];
			}
			set
			{
				this[nameof(txtUProductionModel)] = (object)value;
			}
		}
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string txtUProductionUnit
		{
			get
			{
				return (string)this[nameof(txtUProductionUnit)];
			}
			set
			{
				this[nameof(txtUProductionUnit)] = (object)value;
			}
		}
	}
}
