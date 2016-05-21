using System;

namespace WebUserControl.UI
{
    public class ItemCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {


        /// <summary>
        /// Empty default constructor.
        /// </summary>
        /// <param name="type"></param>
        public ItemCollectionEditor(Type type)
            : base(type)
        { }



        /// <summary>
        /// Provides a list of available item types.
        /// </summary>
        /// <returns>List of <see cref="ToolbarItem"/> objects.</returns>
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
      {
        typeof(ToolbarLink),
        typeof(ToolbarButton),
        typeof(ToolbarImage),
        typeof(ToolbarTextBox),
        typeof(ToolbarLabel),
        typeof(ToolbarSeparator),
        typeof(ControlContainerItem),
      };
        }


    }
}
