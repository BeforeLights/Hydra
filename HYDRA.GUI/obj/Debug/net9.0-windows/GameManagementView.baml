   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5      %clr-namespace:HYDRA.GUI	HYDRA.GUI  5      "    HYDRA.GUI.GameManagementView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:HYDRA.GUI     5      .
convertersclr-namespace:HYDRA.GUI     5         PrimaryDark#��[  5
        � 	Resources  5      $   HYDRA.GUI.BoolToColorConverter  BoolToColorConverter#   HYDRA.GUI.BoolToTextConverter  BoolToTextConverter��%:   &	       &	        5   
   6*     5   
   6)   5      .��5      � $��30q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0,0,0,30q�6"   .+�5      �� $��🎮 Game Management��6     	TitleText#��[ 5      $
��Left=�5      $��28(�5      $��0,0,0,10q�5      �� 5      $H��BManage your game catalog, add new games, and update existing ones.��6     	LabelText#��[ 5         	TextMuted#��[ 5!      $��16(�5"      5#      �� 5&   
   $��1��6     Card#��[ 5'      $��0,0,0,20q�5(       �� Padding$ 20,15q�5)      ._�5*      ��  �� Orientation$ 
Horizontal=�6   .+�5+      �� -   5         ��Name$ AddNewGameButton��5+      $��➕ Add New Game��5,        ModernButton#��[ 5-      $��20,10q�5.      $��14(�5/      $��0,0,15,0q�50      51      �� 53      -   5        $ EditGameButton��53      $��✏️ Edit Selected��54        SecondaryButton#��[ 55      $��20,10q�56      $��14(�57      $��0,0,15,0q�58      59      �� 5;      -   5        $ DeleteGameButton��5;      $��🗑️ Delete Selected��5<       	 DangerButton#��[	 5=      $��20,10q�5>      $��14(�5?      5@      5A      5B      �� 5E   
   $��2��6   #��[ 5F      ._�5G      &   System.Windows.Controls.DataGrid  -   5          Name$ GamesDataGrid��5G       
 ModernDataGrid#��[
 5H         AutoGenerateColumns$ False��5I         SelectionMode$ Single=�5J        ModernDataGridColumnHeader   ColumnHeaderStyle# [ 5K      	   GridLinesVisibility$	 
Horizontal=�5L      
   HorizontalGridLinesBrush
 �#404040   AlternatingRowBackground �#2A2A2A   RowBackground
 ����    Columns 5P      0  *System.Windows.Controls.DataGridTextColumn  5Q      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 5S   '      HeaderStyle# [ 5T   '   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5R   '   �� *GameId+   ElementStyle 5U      �� 5V       �� 
TargetType" ��6$   .�5W   "   ��  �� Property �@��Padding �� Value$
 12,8q�6<   �� 5X   "    � ��  TextSecondary# [ 6?   5Y      5[        5]      $ Title��6)   $ * 5_   '   # [ 5`   '    5^   '   �� *Title+ 5a      �� 5b      " ��6$   .�5c   "   ��  �@��Padding$
 12,8q�6<   �� 5d   "    � ��$ SemiBold"�6?   �� 5e   "    � ��  TextPrimary# [ 6?   5f      5h        5j      $ Price��6)   $	 100 5l   '   # [ 5m   '    5k   '   �� *Price+ �� StringFormat$ C�� 5n      �� 5o      " ��6$   .�5p   "   ��  �@��Padding$
 12,8q�6<   �� 5q   "    � ��$
 Bold"�6?   �� 5r   "    � ��  SuccessGreen# [ 6?   5s      5u        5w      $ Release Date��6)   $	 120 5y   '   # [ 5z   '    5x   '   �� *ReleaseDate+$ d�� 5{      �� 5|      " ��6$   .�5}   "   ��  �@��Padding$
 12,8q�6<   �� 5~   "    � ��# [ 6?   5      5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�      $ For Sale��6-   $	 110 5�   +   # [ 5�   +      CellTemplate 5�      �� 5�      .R�5�   "   �� $
 12,8q�6)   ._�5�   &   ��  �� CornerRadius$ 12��5�   -   $	 8,4q�5�   -   ��5�   -   �� *	IsForSale+ �� 	Converter# [ ._�5�   *   �� 
�������$��12(�5�   4   $��SemiBold"�5�   4   $��Center=�5�   4   ��5�   4   �� *	IsForSale+# [ 5�   4   5�   '   5�   #   5�      5�      5�      5�      5�      5�      