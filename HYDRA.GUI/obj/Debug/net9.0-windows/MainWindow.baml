   M S B A M L   `   `   `  ����D  @HYDRA.GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HYDRA.GUI	HYDRA.GUI  5   	       HYDRA.GUI.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:HYDRA.GUI     5   	     � Title$  HYDRA - Gaming Platform��5   	   $	��800��5	   	   $
��1200��5
   	   $	��600��5   	   $	��800��5   	    � WindowState$ 	Maximized=�5   	      PrimaryDark#��[  5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6     SecondaryDark#��[ 5       �� Padding$ 20,15q�5       J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6#      	Direction$	 270Q�5   !      ShadowDepth$ 2Q�5   !      
BlurRadius$ 8Q�5    !   	   Opacity$		 0.3Q�5!   !   ._�5#      � 
 � ColumnDefinitions
 5$      �� 5%      $
��Auto �6'   �� 5'      $
��Auto �6'   �� 5)      $��* �6'   �� 5+      $
��Auto �6'   .+�50      �� $��0��6    �� Orientation$ 
Horizontal=�51      $��Center=�52      .+�54      �� $��40��6   $��40��6'   $��0,0,15,0q�55      $-��'pack://application:,,,/Images/HYDRA.png��56       �� BitmapScalingMode$ HighQuality=�57      58      �� 5;      -   5         ��Name$ 
BackButton��5<      $��1��5;      $��← Back��5=        SecondaryButton#��[ 5>      $y�	Collapsed=�5?      $��20,0,0,0q�5@      5A      �� 5D      -   5         ��Name$ HeaderPanel��5E      $��2��5D      $ 
Horizontal=�5F      $
��Left=�5G      $��Center=�5H      $��20,0,0,0q�5I      �� 5L      $��3��6   $ 
Horizontal=�5M      $��Center=�5N      .+�5P      �� -   5         ��Name$ WelcomeText��5P         TextSecondary#��[ 5Q       $��14(�5R       $��Center=�5S       $��0,0,15,0q�5T       �� 5W      -   5        $ LogoutButton��5W      $��Sign Out��5X        DangerButton#��[ 5Y      $
��15,8q�5Z      5[      5\      5]      5^      �� 5a   
   $��1��6   #��[  5b      $ 20q�5c      ._�5d      �� -   5         ��Name$ MainContentArea��5d      
������ 5e      5f      5g      5h      