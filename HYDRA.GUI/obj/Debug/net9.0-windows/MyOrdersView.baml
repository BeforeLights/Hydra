   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5          HYDRA.GUI.MyOrdersView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:HYDRA.GUI     5         PrimaryDark#��[  5	      .��5      � $��30q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0,0,0,30q�6"   .+�5      �� $��📋 My Order History��6     	TitleText#��[ 5      $
��Left=�5      $��32(�5      $��0,0,0,10q�5      �� 5      $<��6View your complete purchase history and order details.��6     	LabelText#��[ 5        	TextMuted#��[ 5      $��16(�5      5      �� 5   
   $��1��6     Card#��[ 5       ._�5!      &   System.Windows.Controls.DataGrid  -   5          Name$ OrdersDataGrid��5!        ModernDataGrid#��[ 5"         AutoGenerateColumns$ False��5#         SelectionMode$ Single=�5$        ModernDataGridColumnHeader   ColumnHeaderStyle# [ 5%         GridLinesVisibility$ 
Horizontal=�5&         HorizontalGridLinesBrush �#404040   AlternatingRowBackground �#2A2A2A   RowBackground
 ���� $	��400��5*      	   Columns	 5+      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Order ID��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 5/   '      HeaderStyle# [ 50   '   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5.   '   �� *	OrderId+   ElementStyle 51      �� 52       �� 
TargetType" ��6$   .�53   "   ��  �� Property �@��Padding �� Value$
 12,8q�6<   �� 54   "    � ��  
AccentBlue# [ 6?   �� 55   "    � ��$
 Bold"�6?   �� 56   "    � ��$ Consolas)�6?   57      59        5<      $
 
Order Date��6)   $	 150 5>   '   # [ 5?   '    5=   '   �� *	OrderDate+ �� StringFormat$ d�� 5@      �� 5A      " ��6$   .�5B   "   ��  �@��Padding$
 12,8q�6<   �� 5C   "    � ��  TextSecondary# [ 6?   �� 5D   "    � ��$ SemiBold"�6?   5E      5G        5J      $
 Total Amount��6)   $	 150 5L   '   # [ 5M   '    5K   '   �� *TotalAmount+$ C�� 5N      �� 5O      " ��6$   .�5P   "   ��  �@��Padding$
 12,8q�6<   �� 5Q   "    � �� 	 SuccessGreen# [	 6?   �� 5R   "    � ��$
 Bold"�6?   �� 5S   "    � ��$ 16(�6=   5T      5V      4  .System.Windows.Controls.DataGridTemplateColumn  5Y      $
 Actions��6-   $	 120 5Z   +   # [ 5[   +      CellTemplate 5\      �� 5]      .R�5^   "   ��  �� Padding$
 12,8q�6)   ._�5_   &   �� -   5        $��📋 View Items��5_   -    
 SecondaryButton#��[
 5`   -   $	��8,4q�5c   -   $��11(�5d   -   ��5a   -   �� *	OrderId+5d   -   5e   #   5f      5h      5j      5k      5l      5m      