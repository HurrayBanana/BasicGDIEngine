using System;

namespace GDITest
{
    internal class SpaceBack : Background
    {
        /// <summary>
        /// reference to game form
        /// </summary>
        private GameForm myboss;

        public SpaceBack(GameForm gameForm)
            : base(".\\Graphics\\background.png")
        {
            myboss = gameForm;
            //add click handler
            myboss.button1.Click += new System.EventHandler(RemoveMe);
        }

        /// <summary>
        /// removes background and hides button once clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveMe(object sender, EventArgs e)
        {
            myboss.button1.Click -= new System.EventHandler(RemoveMe);
            myboss.button1.Hide();
            Engine.GDI.RemoveBackground(this);
        }

    }
}