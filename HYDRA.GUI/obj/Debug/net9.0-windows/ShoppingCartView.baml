   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5           HYDRA.GUI.ShoppingCartView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:HYDRA.GUI     5         PrimaryDark#��[  5	      .��5      � $��30q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,30q�6"   .+�5      �� $��🛍️ Shopping Cart��6     	TitleText#��[ 5      $
��Left=�5      $��32(�5      $��0,0,0,10q�5      �� 5      $1��+Review your selected games before checkout.��6     	LabelText#��[ 5        	TextMuted#��[ 5      $��16(�5      5      �� 5    
   $��1��6     Card#��[ 5!      $��0,0,0,20q�5"      ._�5#      � .+�5%      &   System.Windows.Controls.DataGrid  -   5          Name$ CartDataGrid��5%        ModernDataGrid#��[ 5&         AutoGenerateColumns$ False��5'         SelectionMode$ Single=�5(        ModernDataGridColumnHeader   ColumnHeaderStyle# [ 5)         GridLinesVisibility$ 
Horizontal=�5*         HorizontalGridLinesBrush �#404040   AlternatingRowBackground �#2A2A2A   RowBackground
 ���� $	��300��5.      	   Columns	 5/      4  .System.Windows.Controls.DataGridTemplateColumn  51      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Game��61      Width5  /System.Windows.Controls.DataGridLengthConverter$	 400 6?      HeaderStyle# [ 52   /      CellTemplate 53      �� 54   "   .R�55   &   � $
��12,8q�6+    � ColumnDefinitions 56   *   �� 57   .   $��80 �6?   �� 58   .   $��* �6?   .+�5<   *   �� $��0��61    �� CornerRadius$ 4��5=   1   $
}�True��5>   1   $��60��5?   1   $��80��6<   $��0,0,15,0q�5@   1   ._�5A   .   � .+�5B   2   �� $��UniformToFill=�5C   8   ��5B   8   �� *Game.CoverArtPath+5C   8   �� 5D   2   ���#404040y�5E   9   �� *Game.CoverArtPath+)   #HYDRA.GUI.NullToVisibilityConverter   Instance �� 	Converter# Z ._�5F   6   �� $
��🎮��6@   $��24(�5G   @   $��Center=�5H   @   $��Center=�5I   @   #��[ 5J   @   5K   3   5L   /   5M   +   �� 5P   *   $��1��65   $��Center=�5Q   4   .+�5R   .   ��   TextPrimary#��[ 5S   8   $��SemiBold"�5T   8   $��16(�5U   8   $��CharacterEllipsis=�5V   8   ��5R   8   �� *
Game.Title+5V   8   �� 5W   .   #��[ 5X   8   $��14(�5Y   8   $��0,4,0,0q�5Z   8   ��5W   8   �� *Game.Publisher.Name+5Z   8   5[   +   5\   '   5]   #   5_      0  *System.Windows.Controls.DataGridTextColumn  5b      $
 Price��6-   $	 120 5d   +   # [ 5e   +   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5c   +   �� *
Game.Price+ �� StringFormat$ C��   ElementStyle 5f      �� 5g   "    �� 
TargetType" ��6(   .�5h   &   ��  �� Property �@��Padding �� Value$
 12,8q�6@   �� 5i   &    � ��$
 Bold"�6C   �� 5j   &    � ��$ 16(�6A   �� 5k   &    � ��  SuccessGreen# [ 6C   5l   #   5n        5q      $
 Actions��61   $	 120 6B   # [ 5r   /    5s      �� 5t   "   .R�5u   &   ��  �� Padding$
 12,8q�6-   ._�5v   *   �� -   5        $��🗑️ Remove��5v   1    	 DangerButton#��[	 5w   1   $
��10,6q�5z   1   $��12(�5{   1   ��5x   1   �� *Game.GameId+5{   1   5|   '   5}   #   5      5�      �� 5�      $��Center=�6   $��Center=�5�      y�5�      �� *Items.Count+ �� ElementName$ CartDataGrid��*   $HYDRA.GUI.CountToVisibilityConverter   Instance# Z .+�5�      �� $
��🛒��6    $��72(�5�       $��Center=�5�       #��[ 5�       $��0,0,0,20q�5�       �� 5�      $��Your cart is empty��6     
 SubtitleText#��[
 5�       $��Center=�5�       #��[ 5�       $��0,0,0,10q�5�       �� 5�      $3��-Add some games from the store to get started!��6    #��[ 5�       $��Center=�5�       $��16(�5�       5�      5�      5�      �� 5�   
   $��2��6   #��[ 5�      ._�5�      �  5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��0��6   $��Center=�5�      .+�5�      �� $��
Cart Total��6      TextSecondary#��[ 5�       $��16(�5�       $��0,0,0,5q�5�       �� 5�      -   5         ��Name$ TotalPriceTextBlock��5�       $��Total: $0.00��5�       #��[ 5�       $��28(�5�       $
��Bold"�5�       5�      �� 5�      -   5         ��Name$ CheckoutButton��5�      $��1��5�      $��💳 Proceed to Checkout��5�        ModernButton#��[ 5�      $��18(�5�      $��30,15q�5�      $��Center=�5�      5�      5�      5�      5�      5�      