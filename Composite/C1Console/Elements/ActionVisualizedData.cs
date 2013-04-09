using Composite.Core.ResourceSystem;


namespace Composite.C1Console.Elements
{
    /// <summary>    
    /// Describe how an action should appear visually (label, icon).
    /// </summary>
    public sealed class ActionVisualizedData
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public ActionVisualizedData() 
        {
            this.ActionCheckedStatus = ActionCheckedStatus.Uncheckable;
            ActivePositions = ElementActionActivePosition.NavigatorTree;
        }

        /// <summary>
        /// Constructs a new instance, copying from an existing instance.
        /// </summary>
        /// <param name="copy"></param>
        public ActionVisualizedData(ActionVisualizedData copy) 
        {
            this.ActionLocation = copy.ActionLocation;
            this.Disabled = copy.Disabled;
            this.Icon = copy.Icon;
            this.Label = copy.Label;
            this.ToolTip = copy.ToolTip;
            this.ActionCheckedStatus = copy.ActionCheckedStatus;
            this.ActivePositions = copy.ActivePositions;
        }


        /// <summary>
        /// Controls if a check box should be shown next to the action command, and if this checkbox is checked.
        /// </summary>
        public ActionCheckedStatus ActionCheckedStatus { get; set; }

        /// <summary>
        /// Label of the action
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// When disabled, the action will be shown but grayed out and cannot be invoked.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// The actions icon
        /// </summary>
        public ResourceHandle Icon { get; set; }

        /// <summary>
        /// The actions tool tip
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Where the action should show up, relative to toher actions.
        /// </summary>
        public ActionLocation ActionLocation { get; set; }

        /// <summary>
        /// Where the action should be shown - when elements are shown in a selection tree (as opposed to the normal navigation tree) some actions may be desired.
        /// Default is NavigatorTree only.
        /// </summary>
        public ElementActionActivePosition ActivePositions { get; set; }
    }
}
