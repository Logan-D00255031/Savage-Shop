using UnityEngine;

namespace GD
{
    public class ScriptableGameObject : ScriptableObject
    {
        #region Fields

        [ContextMenuItem("Reset Description", "ResetDescription")]
        [TextArea(2, 4)]
        public string Description = string.Empty;

        #endregion Fields

        #region Core Methods

        /// <summary>
        /// Resets the description...obviously
        /// </summary>
        public void ResetName()
        {
            Description = "";
        }

        /// <summary>
        /// Overridden in child classes to specify what a reset means (e.g. clear the list, reset the int, empty the string
        /// </summary>
        public virtual void Reset()
        {
            //noop - (no operation - not called)
        }

        #endregion Core Methods
    }
}