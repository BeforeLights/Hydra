   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5      "    HYDRA.GUI.UserManagementView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:HYDRA.GUI     5         PrimaryDark#��[  5	      .��5      � $��30q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,30q�6"   .+�5      �� $��👥 User Management��6     	TitleText#��[ 5      $
��Left=�5      $��32(�5      $��0,0,0,10q�5      �� 5      $7��1Manage user accounts and roles within the system.��6     	LabelText#��[ 5        	TextMuted#��[ 5      $��16(�5      5      �� 5    
   $��1��6     Card#��[ 5!      $��0,0,0,20q�5"      ._�5#      &   System.Windows.Controls.DataGrid  -   5          Name$ UsersDataGrid��5#        ModernDataGrid#��[ 5$         AutoGenerateColumns$ False��5%         SelectionMode$ Single=�5&        ModernDataGridColumnHeader   ColumnHeaderStyle# [ 5'         GridLinesVisibility$ 
Horizontal=�5(         HorizontalGridLinesBrush �#404040   AlternatingRowBackground �#2A2A2A   RowBackground
 ���� $	��400��5,      	   Columns	 5-      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 51   '      HeaderStyle# [ 52   '   1  +System.Windows.Controls.DataGridBoundColumn   Binding 50   '   �� *UserId+   ElementStyle 53      �� 54       �� 
TargetType" ��6$   .�55   "   ��  �� Property �@��Padding �� Value$
 12,8q�6<   �� 56   "    � ��  TextSecondary# [ 6?   �� 57   "    � ��$ SemiBold"�6?   58      5:        5=      $
 Username��6)   $	 200 5?   '   # [ 5@   '    5>   '   �� *
Username+ 5A      �� 5B      " ��6$   .�5C   "   ��  �@��Padding$
 12,8q�6<   �� 5D   "    � ��$
 Bold"�6?   �� 5E   "    � ��  TextPrimary# [ 6?   5F      5H        5K      $
 Email��6)   $ * 5M   '   # [ 5N   '    5L   '   �� *Email+ 5O      �� 5P      " ��6$   .�5Q   "   ��  �@��Padding$
 12,8q�6<   �� 5R   "    � ��# [ 6?   5S      5U      4  .System.Windows.Controls.DataGridTemplateColumn  5X      $

 Role��6-   $	 150 5Y   +   # [ 5Z   +      CellTemplate 5[      �� 5\      .R�5]   "   ��  �� Padding$
 12,8q�6)   ._�5^   &   ��  �� CornerRadius$ 12��6-   $	 8,4q�5_   -   ��5`   *   �� 5a   .   " ��64    �� Triggers 5b   2   �� 5c   6    �� Binding 6B   �� *Role.RoleName+ �� Value	 Admin6d   .a�5d   :   ��  � �� 	 	DangerRed# [	 6W   5e   7   �� 5f   6    6B   �� *Role.RoleName+ Manager6d   .a�5g   :   ��  � �� 
 WarningOrange# [
 6W   5h   7   �� 5i   6    6B   �� *Role.RoleName+ Customer6d   .a�5j   :   ��  � ��  
AccentBlue# [ 6W   5k   7   5m   /   ._�5o   *   �� 
�������$��12(�5q   4   $��SemiBold"�5r   4   $��Center=�5s   4   ��5o   4   �� *Role.RoleName+5s   4   5t   '   5u   #   5v      5x        5{      $
 Created��6)   $	 120 5}   '   # [ 5~   '    5|   '   �� *DateCreated+ �� StringFormat$ d�� 5      �� 5�      " ��6$   .�5�   "   ��  �@��Padding$
 12,8q�6<   �� 5�   "    � ��# [ 6?   �� 5�   "    � ��$ 12(�6=   5�      5�      5�      5�      �� 5�   
   $��2��6   #��[ 5�      ._�5�      �  � ColumnDefinitions 5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��0��6    �� Orientation$ 
Horizontal=�5�      $��Center=�5�      .+�5�      �� $"��Change Selected User's Role:��6    #��[ 5�       $��SemiBold"�5�       $��Center=�5�       $��0,0,15,0q�5�       �� 5�      -   5         ��Name$ RoleComboBox��5�       �� DisplayMemberPath$ RoleName��5�       �� SelectedValuePath$ RoleId��5�        ModernComboBox#��[ 5�      $	��180��5�      5�      �� 5�      -   5         ��Name$ SaveChangesButton��5�      $��1��5�      $��💾 Save Role Change��5�        ModernButton#��[ 5�      $��20,10q�5�      $
��15,0q�5�      5�      �� 5�      -   5        $ DeleteUserButton��5�      $��2��5�      $"��🗑️ Delete Selected User��5�        DangerButton#��[ 5�      $��20,10q�5�      5�      5�      5�      5�      5�      