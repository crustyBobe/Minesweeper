using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Cell UserControl to act as the cells on the grid in MinesweeperForm.cs. Contains a panel, button, and label. The button disappears when
/// clicked, and the label contains text that is either empty, a * for a mine, or a number to show the number of mines in adjacent cells.
/// Has properties to get the row and cell values of the cell for when placing it on the form.
/// </summary>

namespace kneal3Minesweeper
{
    public partial class Cell : UserControl
    {
        //Event to reference for when the cell is clicked to determine if there are other blank cells nearby.
        public EventHandler CellHasBeenClicked;

        //Set the size of the cell with an integer to easily change the size if we need to later.
        private int cellSize = 24;

        //Create a global panel for the cell.
        public Panel panel = new Panel();

        //Create a new global button for the cell.
        public Button button = new Button();

        //Create a global label to hold the number of mines nearby or the mine on top of the panel.
        public Label label = new Label();

        //Variables to hold the row and column of the cell that was clicked.
        int row = 0;
        int col = 0;

        /// <summary>
        /// Default setup for the Cell. Calls InitializeComponent and SetupCell.
        /// </summary>
        public Cell()
        {
            InitializeComponent();
            SetupCell();
        }//end Cell.

        //Properties for the cell's variables.
        #region Properties
        
        //Property to get the size of the cell.
        public int CellSize { get => cellSize; set => cellSize = value; }

        //Property to get the cell's button.
        public Button Button { get => button; }

        //Property to get and set the row of the button within the grid.
        public int Row { get => row; set => row = value; }

        //Property to get and set the column of the button within the grid.
        public int Col { get => col; set => col = value; }

        #endregion

        //Method to setup the cell's color and size.
        #region Cell Setup

        /// <summary>
        /// Setup the cell by setting the size, colors, and numbers if there are bombs nearby.
        /// </summary>
        public void SetupCell()
        {
            //Sets the cell size using the cellSize variable.
            this.Size = new Size(CellSize, CellSize);

            //Sets the size of the panel and button using cellSize.
            panel.Size = new Size(CellSize, CellSize);
            button.Size = new Size(CellSize, CellSize);

            //Set the back color of the panel and button.
            button.BackColor = Color.FromArgb(192, 192, 192);
            panel.BackColor = Color.FromArgb(255, 223, 223);

            //Sets the border of the button.
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;

            //Add the button click handler to the button for when it is clicked.
            button.Click += OnButtonClickHandler;

            //Add the button and panel to the cell.
            this.Controls.Add(button);
            this.Controls.Add(label);
            this.Controls.Add(panel);
        }//end SetupCell.

        #endregion

        //Handler for if the cell is clicked.
        #region Handler

        /// <summary>
        /// Click handler for the button to set it's visibility to false when it is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnButtonClickHandler(object sender, EventArgs e)
        {
            //Sets the button's visibility to false when it is clicked.
            button.Visible = false;

            //If there is something in the CellHasBeenClicked handler.
            if(CellHasBeenClicked != null)
            {
                //Pass this to the handler to click the cell.
                this.CellHasBeenClicked(this, e);
            }
        }//end OnButtonClickHandler.

        #endregion

    }//end Cell partial class.
}