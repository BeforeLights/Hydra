   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5          HYDRA.GUI.App   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      )localclr-namespace:HYDRA.GUI     5        �� 	Resources  5         PrimaryDark �� Color  SecondaryDark  
AccentBlue  AccentBlueHover  TextPrimary  TextSecondary  	TextMuted  	DangerRed  SuccessGreen 	 WarningOrange 
 ModernButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Padding �� Triggers	 P� Property
 P� Value  SecondaryButton �� BasedOn �� Triggers  DangerButton  ModernTextBox ��Name  ModernPasswordBox  ModernDataGrid&   System.Windows.Controls.DataGrid  ModernDataGridColumnHeader=  7System.Windows.Controls.Primitives.DataGridColumnHeader  ModernListBox  ModernComboBox d�Name �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource � ColumnDefinitions ��Name J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �Name �� IsDropDownOpen � AllowsTransparency �Name  J� SnapsToDevicePixels! � ActualWidth" �� MaxDropDownHeight# ��Name$ K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation%   DirectionalNavigation& �� 
TargetName  Card3  -System.Windows.Media.Effects.DropShadowEffect'   Color(   	Direction)   ShadowDepth*   
BlurRadius+   Opacity  	TitleText  SubtitleText  	LabelText��%V  &	        &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &	 �     &	     &		 )    &	
 J    7  7  7  &	 �    7 
 &	 x    7 
 7  &	 6    7  7  &	 �    7  7  &	 �
    7   7  &	 �    7  7  &	 �    7   7  &	 �    7  7  7  7  7  7  7  &	 �    7  &	     7  &	 �    7  &	 K    7  �� 5   
   $ #1A1A1A��6.   �� 5	   
   $ #2D2D30��60   �� 5
   
   $ #007ACC��6-   �� 5   
   $ #0099FF��62   �� 5   
   $ #FFFFFF��6.   �� 5   
   $ #CCCCCC��60   �� 5   
   $ #999999��6,   �� 5   
   $ #DC3545��6,   �� 5   
   $ #28A745��6/   �� 5   
   $ #FFC107��60   �� 5   
   " ��6%   .�5      ��  � ��8   6+   �� 5       � ��8  6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 16,8q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 14(�6)   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5       �� #��z��6!   $ 4��5!   !   # z��5"   !   ._�5#      �� $��Center=�6/   $��Center=�5$   -   5%       5&      P� 5'      	 �@J�IsMouseOver$

 True��6=   . �5(   "   �� 8   � ��6?   5)      P� 5*      	 � ��$

 True��6;   . �5+   "   ��  � �� �#005A9E6?   5,      P� 5-      	 � {�$
 False��6;   . �5.   "   ��  � �� �#5555556?   �� 5/   "    � �� �#9999996?   50      52      54      55      �� 58   
   " ��6(   8   .�59      ��  � �� �#6C757D6+    5:      P� 5;      	 �@J�IsMouseOver$

 True��61   . �5<      ��  � �� �#5A626863   5=      5?      �� 5B   
   " ��6%   8   .�5C      ��  � ��8  6+    5D      P� 5E      	 �@J�IsMouseOver$

 True��61   . �5F      ��  � �� �#C8233363   5G      5I      �� 5L   
   " ��6&   .�5M      ��  � �� �#3C3C3C6+   �� 5N       � ��8   6+   �� 5O       � �� �#5555556,   �� 5P       � ��$ 1q�60   �� 5Q       � ��$
 12,8q�6(   �� 5R       � ��$ 14(�6)   �� 5S       � �� 5T      �� 5U      " ��6&   .R�5V      �� #��z��6!   #��z��5W   !   #��z��5X   !   $ 4��5Y   !   ._�5Z      �� $ PART_ContentHost��6+   #��z��5[   )   5\       5]      P� 5^      	 �@J�	IsFocused$

 True��6;   . �5_   "   �� 8   � ��6@   5`      5b      5d      5e      �� 5h   
   " G�6*   .�5i      ��  � �� �#3C3C3C6+   �� 5j       � ��8   6+   �� 5k       � �� �#5555556,   �� 5l       � ��$ 1q�60   �� 5m       � ��$
 12,8q�6(   �� 5n       � ��$ 14(�6)   �� 5o       � �� 5p      �� 5q      " G�6&   .R�5r      �� #��z��6!   #��z��5s   !   #��z��5t   !   $ 4��5u   !   ._�5v      �� $ PART_ContentHost��6+   #��z��5w   )   5x       5y      P� 5z      	 �@J�	IsFocused$

 True��6;   . �5{   "   �� 8   � ��6@   5|      5~      5�      5�      �� 5�   
   "  6'   .�5�      ��  � ��8   6+   �� 5�       � ��8  6+   �� 5�       � �� �#5555556,   �� 5�       � ��$ 1q�60   �� 5�       �@ GridLinesVisibility$ 
Horizontal=�64   �� 5�        �@ HorizontalGridLinesBrush �#55555569   �� 5�        �@ AlternatingRowBackground �#33333369   �� 5�       �@ RowBackground
 ���� 6.   �� 5�       �@ HeadersVisibility$ Column=�62   �� 5�       �@ AutoGenerateColumns$ False��64   �� 5�       �@ CanUserAddRows$ False��6/   �� 5�       �@ CanUserDeleteRows$ False��62   �� 5�       �@ 
IsReadOnly$
 True��6+   5�      �� 5�   
   "  63   .�5�      ��  � ��8   6+   �� 5�       � ��8  6+   �� 5�       � ��$ SemiBold"�6+   �� 5�       � ��$
 12,8q�6(   �� 5�       � �� �#5555556,   �� 5�       � ��$ 0,0,1,1q�60   5�      �� 5�   
   " ��6&   .�5�      ��  � ��8   6+   �� 5�       � ��8  6+   �� 5�       � �� �#5555556,   �� 5�       � ��$ 1q�60   �� 5�       � ��$ Disabled=�6K   �� 5�       � ��$
 Auto=�6I   5�      �� 5�   
   " ��6'   .�5�      ��  � �� �#3C3C3C6+   �� 5�       � ��8   6+   �� 5�       � �� �#5555556,   �� 5�       � ��$ 1q�60   �� 5�       � ��$
 12,8q�6(   �� 5�       � ��$ 14(�6)   �� 5�       � �� 5�      �� 5�      " ��6&   .R�5�      � .+�5�      d� $ ToggleButton��6+   #��z��5�   )   #��z��5�   )   #��z��5�   )   $��0��5�   )   $|�False��5�   )   $ Press=�5�   )    5�   )   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+��5�   "   �� 5�   &   " d�66   .R�5�   *   �� #��z��61   #��z��5�   /   #��z��5�   /   $ 4��5�   /   ._�5�   .   �  5�   2   �� 5�   6   $��* �6G   �� 5�   6   $��20 �6G   .+�5�   2   F� $��1��67   8�� $��Center=�5�   5   $��Center=�5�   5   "����        � 	=  	= � z     	5�   5   5�   /   5�   +    5�   *   P� 5�   .   	 �@J�IsMouseOver$

 True��6M   . �5�   2   �� 8   � ��6P   5�   /   5�   '   5�      �� 5�      $ ContentSite��6/   $ False��5�   -   #��z 5�   -   #��z 5�   -   #��z��5�   -   #��z��5�   -   $��Center=�5�   -   $
��Left=�5�   -   �� 5�      $ PART_EditableTextBox��6&   $
��Left=�5�   $   $��Center=�5�   $   #��z��5�   $   $
|�True��5�   $   
������ #��z��5�   $   $y�Hidden=�5�   $   # z 5�   $   ��5�   $   Z� 5�   $   � 5�      $ Popup��6$   $��Bottom=�5�   "   #��z 5�   "   $
 True��5�   "   $|�False��5�   "   $��Slide=�5�   "   .��5�   "   � $ DropDown��6'   $
  True��5�   %   #��z! 5�   %   #��z" 5�   %   .+�5�   &   �� $# DropDownBorder��6-   8�� ���#555555$��1q�5�   +   $ 4��5�   +   $��0,1,0,0q�5�   +   ._�5�   &   �� $	��4,6q�63   $
  True��5�   1   .��5�   *   �� $
$ True��65   $% 	Contained=�5�   3   5�   '   5   '   5  #   5     5      5     P� 5     	 �@��
IsEditable$

 True��6<   . �5  "   ��  � ��$ False��6>   �� 5  "   $& PART_EditableTextBox��6?    � y�$ Visible=�6a   �� 5  "   $& ContentSite��6?    � y�$ Hidden=�6X   5	     5     5     �� 5      � �� 5     �� 5     " ��6   .�5     ��  � ��
 ���� 67   �� 5      � ��8  67   �� 5      � ��$
 12,8q�64   �� 5      � ��$ 14(�65   �� 5      � �� 5     �� 5  "   " ��62   .R�5  &   �� #��z��6-   # z��5  +   ._�5  &   �� 5  '    5  &   P� 5  *   	 �@��IsHighlighted$

 True��6K   . �5  .   �� 8   � ��6K   �� 5  .    � ��
 �����6K   5   +   P� 5!  *   	 �@J�IsMouseOver$

 True��6I   . �5"  .   �� 8   � ��6K   �� 5#  .    � ��
 �����6K   5$  +   5&  #   5(     5)     5+     5,     �� 5/  
   " ��6   .�50     ��  � ��8   6+   �� 51      �@��CornerRadius$ 8��6-   �� 52      �@��Padding$ 20q�6(   �� 53      � ��$ 10q�6'   �� 54      �@J�Effect 55       56     $' Black��6'   $	( 270Q�57  %   $) 2Q�58  %   $* 8Q�59  %   $	+ 0.3Q�5:  %   5<     5=     �� 5@  
   " ��6"   .�5A     ��  � ��8   6+   �� 5B      � ��$ 32(�6)   �� 5C      � ��$
 Bold"�6+   �� 5D      � ��$ Center=�64   5E     �� 5H  
   " ��6%   .�5I     ��  � ��8   6+   �� 5J      � ��$ 18(�6)   �� 5K      � ��$ SemiBold"�6+   5L     �� 5O  
   " ��6"   .�5P     ��  � ��8   6+   �� 5Q      � ��$ 14(�6)   �� 5R      � ��$ Medium"�6+   �� 5S      � ��$ 0,0,0,5q�6'   5T     5U     5V     