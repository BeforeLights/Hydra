   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5   	        HYDRA.GUI.GameEditorWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:HYDRA.GUI     5   	     � Title$  GameEditorWindow��5   	   $	��450��6"   $	��800��6/   .��5	      � ���#2D2D30$��15q�6     � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Game Editor��6!   $��24(�64   $
��Bold"�6B   
�������$��0,0,0,20q�6g   �� 5   
   $��1��6   $
��Auto=�6$   .��5      �� .+�5      �� $��Title��6   
�������$��0,0,0,5q�6<   �� 5      -   5         ��Name$ TitleTextBox��5      $��5q�60   $��0,0,0,15q�6<   �� 5      $��Description��6   
�������$��0,0,0,5q�6B   �� 5      -   5        $ DescriptionTextBox��5      $��5q�66   $��0,0,0,15q�6B    �� TextWrapping$
 Wrap=�6T    �� AcceptsReturn$
 True��6h   $��80��6}   �� 5      $��Price��6   
�������$��0,0,0,5q�6<   �� 5      -   5        $ PriceTextBox��5      $��5q�60   $��0,0,0,15q�6<   �� 5      $��Release Date��6   
�������$��0,0,0,5q�6C   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ ReleaseDatePicker��5      $��5q�68   $��0,0,0,15q�6D   �� 5       $��Cover Art Path��6   
�������$��0,0,0,5q�6E   �� 5!      -   5        $ CoverArtPathTextBox��5!      $��5q�67   $��0,0,0,15q�6C   �� 5#      -   5         ��Name$ IsForSaleCheckBox��5#      $��Is For Sale��66   
������� d� 	IsChecked$
 TrueY�6_   $��0,0,0,15q�6p   5$      5%      �� 5'   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,20,0,0q�6W   .+�5(      �� -   5        	 ��Name$	 
SaveButton��5(      $
��Save��6)   $	��100��68   $��8q�6D   $��0,0,10,0q�6P   ���#007ACC
�������6�   �� 5)      -   5        $	 CancelButton��5)      $��Cancel��6+   $	��100��6<   $��8q�6H   
 �� IsCancel$

 True��6T   5*      5+      5,      