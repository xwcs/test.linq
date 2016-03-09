using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Permissions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Security;

namespace WF2
{
/*
	public abstract class ChainingPropertyDescriptor : PropertyDescriptor
	{
		private Type _forcedType = null;
		private readonly PropertyDescriptor _root;
		protected PropertyDescriptor Root { get { return _root; } }
		protected ChainingPropertyDescriptor(PropertyDescriptor root)
			: base(root)
		{
			_root = root;
		}
		public override void AddValueChanged(object component, EventHandler handler)
		{
			Root.AddValueChanged(component, handler);
		}
		public override AttributeCollection Attributes
		{
			get
			{
				return Root.Attributes;
			}
		}
		public override bool CanResetValue(object component)
		{
			return Root.CanResetValue(component);
		}
		public override string Category
		{
			get
			{
				return Root.Category;
			}
		}
		public override Type ComponentType
		{
			get { return Root.ComponentType; }
		}
		public override TypeConverter Converter
		{
			get
			{
				return Root.Converter;
			}
		}
		public override string Description
		{
			get
			{
				return Root.Description;
			}
		}
		public override bool DesignTimeOnly
		{
			get
			{
				return Root.DesignTimeOnly;
			}
		}
		public override string DisplayName
		{
			get
			{
				return Root.DisplayName;
			}
		}
		public override bool Equals(object obj)
		{
			return Root.Equals(obj);
		}
		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return Root.GetChildProperties(instance, filter);
		}
		public override object GetEditor(Type editorBaseType)
		{
			return Root.GetEditor(editorBaseType);
		}
		public override int GetHashCode()
		{
			return Root.GetHashCode();
		}
		public override object GetValue(object component)
		{
			return Root.GetValue(component);
		}
		public override bool IsBrowsable
		{
			get
			{
				return Root.IsBrowsable;
			}
		}
		public override bool IsLocalizable
		{
			get
			{
				return Root.IsLocalizable;
			}
		}
		public override bool IsReadOnly
		{
			get { return Root.IsReadOnly; }
		}
		public override string Name
		{
			get
			{
				return Root.Name;
			}
		}
		public override Type PropertyType
		{
			get {
				Console.WriteLine("Type:" + (_forcedType == null ? Root.PropertyType : _forcedType).ToString());
				return _forcedType == null ? Root.PropertyType : _forcedType; 
			}
		}

		public Type ForcedPropertyType
		{
			get { return _forcedType == null ? Root.PropertyType : _forcedType; }
			set { _forcedType = value; }
		}

		public override void RemoveValueChanged(object component, EventHandler handler)
		{
			Root.RemoveValueChanged(component, handler);
		}
		public override void ResetValue(object component)
		{
			Root.ResetValue(component);
		}
		public override void SetValue(object component, object value)
		{
			Root.SetValue(component, value);
		}
		public override bool ShouldSerializeValue(object component)
		{
			return Root.ShouldSerializeValue(component);
		}
		public override bool SupportsChangeEvents
		{
			get
			{
				return Root.SupportsChangeEvents;
			}
		}
		public override string ToString()
		{
			return Root.ToString();
		}
	}


	
	public class TestDescriptor : ChainingPropertyDescriptor {
		public TestDescriptor(PropertyDescriptor root) : base(root) { }
	}
	

	sealed class HyperTypeDescriptor : CustomTypeDescriptor
	{
		private readonly PropertyDescriptorCollection propertyCollections;
		static readonly Dictionary<PropertyInfo, PropertyDescriptor> properties = new Dictionary<PropertyInfo, PropertyDescriptor>();


		internal HyperTypeDescriptor(ICustomTypeDescriptor parent) :
			base(parent)
		{
			propertyCollections = WrapProperties(parent.GetProperties());
		}
		public sealed override PropertyDescriptorCollection GetProperties(
			Attribute[] attributes)
		{
			return propertyCollections;
		}
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return propertyCollections;
		}
		private static PropertyDescriptorCollection WrapProperties(
			PropertyDescriptorCollection oldProps)
		{
			PropertyDescriptor[] newProps = new PropertyDescriptor[oldProps.Count];
			int index = 0;
			bool changed = false;
			// HACK: how to identify reflection, given that the class is internal
			Type wrapMe = Assembly.GetAssembly(typeof(PropertyDescriptor)).
				GetType("System.ComponentModel.ReflectPropertyDescriptor");
			foreach (PropertyDescriptor oldProp in oldProps)
			{
				newProps[index++] = new TestDescriptor(oldProp);
				changed = true;
				
			}

			//PropertyDescriptorCollection tmp = new PropertyDescriptorCollection(newProps, false);
			
			return changed ? new PropertyDescriptorCollection(newProps, false) :	oldProps;
		}

		static readonly ModuleBuilder moduleBuilder;
		static int counter;
		static HyperTypeDescriptor()
		{
			AssemblyName an = new AssemblyName("Hyper.ComponentModel.dynamic");
			AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
			moduleBuilder = ab.DefineDynamicModule("Hyper.ComponentModel.dynamic.dll");

		}

		private static bool TryCreatePropertyDescriptor(ref PropertyDescriptor descriptor)
		{
			try
			{
				PropertyInfo property = descriptor.ComponentType.GetProperty(descriptor.Name);
				if (property == null) return false;

				lock (properties)
				{
					PropertyDescriptor foundBuiltAlready;
					if (properties.TryGetValue(property, out foundBuiltAlready))
					{
						descriptor = foundBuiltAlready;
						return true;
					}

					string name = "_c" + Interlocked.Increment(ref counter).ToString();
					TypeBuilder tb = moduleBuilder.DefineType(name, TypeAttributes.Sealed | TypeAttributes.NotPublic | TypeAttributes.Class | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoClass | TypeAttributes.Public, typeof(ChainingPropertyDescriptor));

					// ctor calls base
					ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { typeof(PropertyDescriptor) });
					ILGenerator il = cb.GetILGenerator();
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldarg_1);
					il.Emit(OpCodes.Call, typeof(ChainingPropertyDescriptor).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(PropertyDescriptor) }, null));
					il.Emit(OpCodes.Ret);

					MethodBuilder mb;
					MethodInfo baseMethod;
					if (property.CanRead)
					{
						// obtain the implementation that we want to override
						baseMethod = typeof(ChainingPropertyDescriptor).GetMethod("GetValue");
						// create a new method that accepts an object and returns an object (as per the base)
						mb = tb.DefineMethod(baseMethod.Name,
							MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final,
							baseMethod.CallingConvention, baseMethod.ReturnType, new Type[] { typeof(object) });
						// start writing IL into the method
						il = mb.GetILGenerator();
						if (property.DeclaringType.IsValueType)
						{
							// upbox the object argument into our known (instance) struct type
							LocalBuilder lb = il.DeclareLocal(property.DeclaringType);
							il.Emit(OpCodes.Ldarg_1);
							il.Emit(OpCodes.Unbox_Any, property.DeclaringType);
							il.Emit(OpCodes.Stloc_0);
							il.Emit(OpCodes.Ldloca_S, lb);
						}
						else
						{
							// cast the object argument into our known class type
							il.Emit(OpCodes.Ldarg_1);
							il.Emit(OpCodes.Castclass, property.DeclaringType);
						}
						// call the "get" method
						il.Emit(OpCodes.Callvirt, property.GetGetMethod());

						if (property.PropertyType.IsValueType)
						{
							// box it from the known (value) struct type
							il.Emit(OpCodes.Box, property.PropertyType);
						}
						// return the value
						il.Emit(OpCodes.Ret);
						// signal that this method should override the base
						tb.DefineMethodOverride(mb, baseMethod);
					}

					bool supportsChangeEvents = descriptor.SupportsChangeEvents, isReadOnly = descriptor.IsReadOnly;

					// override SupportsChangeEvents
					baseMethod = typeof(ChainingPropertyDescriptor).GetProperty("SupportsChangeEvents").GetGetMethod();
					mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, Type.EmptyTypes);
					il = mb.GetILGenerator();
					if (supportsChangeEvents)
					{
						il.Emit(OpCodes.Ldc_I4_1);
					}
					else
					{
						il.Emit(OpCodes.Ldc_I4_0);
					}
					il.Emit(OpCodes.Ret);
					tb.DefineMethodOverride(mb, baseMethod);

					// override IsReadOnly
					baseMethod = typeof(ChainingPropertyDescriptor).GetProperty("IsReadOnly").GetGetMethod();
					mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, Type.EmptyTypes);
					il = mb.GetILGenerator();
					if (isReadOnly)
					{
						il.Emit(OpCodes.Ldc_I4_1);
					}
					else
					{
						il.Emit(OpCodes.Ldc_I4_0);
					}
					il.Emit(OpCodes.Ret);
					tb.DefineMethodOverride(mb, baseMethod);

					
                    // override PropertyType
                    baseMethod = typeof(ChainingPropertyDescriptor).GetProperty("PropertyType").GetGetMethod();
                    mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, Type.EmptyTypes);
                    il = mb.GetILGenerator();
                    il.Emit(OpCodes.Ldtoken, descriptor.PropertyType);
                    il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                    il.Emit(OpCodes.Ret);
                    tb.DefineMethodOverride(mb, baseMethod);

					/*  REMOVED:  ComponentType; actually *adds* time overriding these
                    // override ComponentType
                    baseMethod = typeof(ChainingPropertyDescriptor).GetProperty("ComponentType").GetGetMethod();
                    mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, Type.EmptyTypes);
                    il = mb.GetILGenerator();
                    il.Emit(OpCodes.Ldtoken, descriptor.ComponentType);
                    il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                    il.Emit(OpCodes.Ret);
                    tb.DefineMethodOverride(mb, baseMethod);
                    * /

					// for classes, implement write (would be lost in unbox for structs)
					if (!property.DeclaringType.IsValueType)
					{
						if (!isReadOnly && property.CanWrite)
						{
							// override set method
							baseMethod = typeof(ChainingPropertyDescriptor).GetMethod("SetValue");
							mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, baseMethod.CallingConvention, baseMethod.ReturnType, new Type[] { typeof(object), typeof(object) });
							il = mb.GetILGenerator();
							il.Emit(OpCodes.Ldarg_1);
							il.Emit(OpCodes.Castclass, property.DeclaringType);
							il.Emit(OpCodes.Ldarg_2);
							if (property.PropertyType.IsValueType)
							{
								il.Emit(OpCodes.Unbox_Any, property.PropertyType);
							}
							else
							{
								il.Emit(OpCodes.Castclass, property.PropertyType);
							}
							il.Emit(OpCodes.Callvirt, property.GetSetMethod());
							il.Emit(OpCodes.Ret);
							tb.DefineMethodOverride(mb, baseMethod);
						}

						if (supportsChangeEvents)
						{
							EventInfo ei = property.DeclaringType.GetEvent(property.Name + "Changed");
							if (ei != null)
							{
								baseMethod = typeof(ChainingPropertyDescriptor).GetMethod("AddValueChanged");
								mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, new Type[] { typeof(object), typeof(EventHandler) });
								il = mb.GetILGenerator();
								il.Emit(OpCodes.Ldarg_1);
								il.Emit(OpCodes.Castclass, property.DeclaringType);
								il.Emit(OpCodes.Ldarg_2);
								il.Emit(OpCodes.Callvirt, ei.GetAddMethod());
								il.Emit(OpCodes.Ret);
								tb.DefineMethodOverride(mb, baseMethod);

								baseMethod = typeof(ChainingPropertyDescriptor).GetMethod("RemoveValueChanged");
								mb = tb.DefineMethod(baseMethod.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.SpecialName, baseMethod.CallingConvention, baseMethod.ReturnType, new Type[] { typeof(object), typeof(EventHandler) });
								il = mb.GetILGenerator();
								il.Emit(OpCodes.Ldarg_1);
								il.Emit(OpCodes.Castclass, property.DeclaringType);
								il.Emit(OpCodes.Ldarg_2);
								il.Emit(OpCodes.Callvirt, ei.GetRemoveMethod());
								il.Emit(OpCodes.Ret);
								tb.DefineMethodOverride(mb, baseMethod);
							}
						}

					}
					PropertyDescriptor newDesc = tb.CreateType().GetConstructor(new Type[] { typeof(PropertyDescriptor) }).Invoke(new object[] { descriptor }) as PropertyDescriptor;
					if (newDesc == null)
					{
						return false;
					}
					descriptor = newDesc;
					properties.Add(property, descriptor);
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}


	public sealed class HyperTypeDescriptionProvider : TypeDescriptionProvider
	{
		public static void Add(Type type)
		{
			TypeDescriptionProvider parent = TypeDescriptor.GetProvider(type);
			TypeDescriptor.AddProvider(new HyperTypeDescriptionProvider(parent), type);
		}
		public HyperTypeDescriptionProvider() : this(typeof(object)) { }
		public HyperTypeDescriptionProvider(Type type) : this(TypeDescriptor.GetProvider(type)) { }
		public HyperTypeDescriptionProvider(TypeDescriptionProvider parent) : base(parent) { }
		public static void Clear(Type type)
		{
			lock (descriptors)
			{
				descriptors.Remove(type);
			}
		}
		public static void Clear()
		{
			lock (descriptors)
			{
				descriptors.Clear();
			}
		}
		private static readonly Dictionary<Type, ICustomTypeDescriptor> descriptors = new Dictionary<Type, ICustomTypeDescriptor>();
		public sealed override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			ICustomTypeDescriptor descriptor;
			lock (descriptors)
			{
				if (!descriptors.TryGetValue(objectType, out descriptor))
				{
					try
					{
						descriptor = BuildDescriptor(objectType);
					}
					catch
					{
						return base.GetTypeDescriptor(objectType, instance);
					}
				}
				return descriptor;
			}
		}


		/*
         * Changes made for .NET 4 compability. Source: http://stackoverflow.com/questions/3105763/does-hyperdescriptor-work-when-built-in-net-4
         *
         * /
		[SecuritySafeCritical]
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private ICustomTypeDescriptor BuildDescriptor(Type objectType)
		{
			// NOTE: "descriptors" already locked here

			// get the parent descriptor and add to the dictionary so that
			// building the new descriptor will use the base rather than recursing
			ICustomTypeDescriptor descriptor = base.GetTypeDescriptor(objectType, null);
			descriptors.Add(objectType, descriptor);
			try
			{
				// build a new descriptor from this, and replace the lookup
				descriptor = new HyperTypeDescriptor(descriptor);
				descriptors[objectType] = descriptor;
				return descriptor;
			}
			catch
			{   // rollback and throw
				// (perhaps because the specific caller lacked permissions;
				// another caller may be successful)
				descriptors.Remove(objectType);
				throw;
			}
		}
	}
	*/


	public class bab_meta
	{
		const string MainGroup = "{tabs}";
		const string Resp = MainGroup + "/Responsabilita";
		const string MG_Main = MainGroup + "/<Doc>/Main";
		const string MG_Parole = MainGroup + "/<Doc>/Parole";
		const string MG_Testo = MainGroup + "/<Doc>/Testo";

		[Display(Name = "n° Doc", GroupName = MG_Main + "/<IdRandom->", Order = 1)]
		[System.ComponentModel.ReadOnly(true)]
		public int ndoc { get; set; }

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "Id", GroupName = MG_Main + "/<IdRandom->")]
		[System.ComponentModel.ReadOnly(true)]
		public Int32 id { get; set; }

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "Random", GroupName = MG_Main + "/<IdRandom->")]
		[Range(0, 1000, ErrorMessage = "Fuori range!")]
		public int random { get; set; }
		[Display(Name = "Parole", GroupName = MG_Parole)]
		public string dictionary { get; set; }
		[Display(Name = "Testo", GroupName = MG_Parole), DataType(DataType.MultilineText)]
		public string text { get; set; }
		[Display(Name = "RPA", GroupName = Resp)]
		public string rpa { get; set; }
		[Display(Name = "CC", GroupName = Resp)]
		[xwcs.core.ui.datalayout.attributes.DbLookup(DisplayMember = "Name", ValueMember = "Name")]
		public string cc { get; set; }
		[Display(Name = "DXP", GroupName = Resp)]
		public string dxp { get; set; }
		[System.ComponentModel.ReadOnly(true)]
		[Display(Name = "xml", GroupName = MainGroup + "/XML"), DataType(DataType.MultilineText)]
		public string xml { get; set; }
		[Display(AutoGenerateField = false)]
		public string extra { get; set; }

		[Display(AutoGenerateField = false)]
		public bab_local bab_local { get; set; }
	}

	public class bab_ext {
		public const UInt32 greenColor = 0xff008000;

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_RPA", GroupName = "n")]
		public string n_rpa { get; set; }
		[Display(Name = "N_CC", GroupName = "n")]
		public string n_cc { get; set; }
		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_DXP", GroupName = "n")]
		public string n_dxp { get; set; }
		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "N_Date", GroupName = "n")]
		public DateTime n_data { get; set; }
	}

	//[TypeDescriptionProvider(typeof(SomeTypeDescriptionProvider<bab>))]
	[MetadataType(typeof(bab_meta))]
	public partial class bab {

		[Display(Name = "Embeded", GroupName = "{tabs}/Embeded", Order = 4)]
		public object Bab_Ext { get; set; } = new bab_ext();
	}




	public partial class bab_view_data
	{
		[Key]
		[Display(Name = "n° Doc")]
		public int ndoc { get; set; }
		[Display(Name = "Id")]
		public Int32 id { get; set; }
		[Display(Name = "Random")]
		public int random { get; set; }
		[Display(Name = "Parole", GroupName = "{tabs}")]
		public string dictionary { get; set; }
	}
}
