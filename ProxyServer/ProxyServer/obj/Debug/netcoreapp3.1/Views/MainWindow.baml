   M S B A M L   `   `   `  ����F  BProxyServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ProxyServerProxyServer  5   	       ProxyServer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ProxyServer     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5	      ��  �� Padding$ 10q�6   ���	#FF222428._�5
   
   �  � ColumnDefinitions 5      �� 5      $��4* �6#   �� 5      $��5* �6#   �� 5      $��3* �6#   .+�5      � $��0��6    � RowDefinitions 5      �� 5      $
��auto �6$   �� 5      $��* �6$   �� 5      $
��auto �6$   .+�5      �� $��ProxyServer_��6   $��0��60   $
��Bold"�6=   $��10q�6O   ���	#FFEEEEEE$��24(�6r   �� 5      -   5        $��LogList��5      $��1��6)   $��10q�66   �� 5      -   5        $��MaakLeeg��5      $��2��6)   $��	Maak Leeg��66   $��10q�6J   $��5q�6V    � Cursor$
 Hand��6b   6p   5       � 5#      $��1��6    5$      �� 5%      $��* �6$   .+�5(      �� -   5        $��HttpDetails��5(      $��1��6-   $��15q�6:   $��0q�6F   5)      � 5,      $��2��6   $��0 0 10 0q�6#   .+�5-      �� $��Stretch=�6   $��Stretch=�69    �� CanHorizontallyScroll$
 True��6W    �� CanVerticallyScroll$
 True��6t   $
��True��6�   .+�5.      ��  �� Orientation$ Vertical=�6!   .+�5/      �� $
��10 0q�6    $	��0 5q�6.   $��Proxy IPAdress��6<   $��Center=�6U   $
��Left=�6p   ���	#FFEEEEEE6�   �� 50      -   5        $��InputServerIP��50   "   $��	127.0.0.1��67   $
��10 0q�6H   $��5q�6V   51      �� 53      $ Vertical=�6!   .+�54      �� $
��10 0q�6    $	��0 5q�6.   $��Proxy poort��6<   $��Center=�6R   $
��Left=�6m   ���	#FFEEEEEE6�   �� 55      -   5        $��InputPortNumber��55   "   $
��8080��69   $
��10 0q�6E   $��5q�6S   56      �� 58      $ Vertical=�6!   .+�59      �� $
��10 0q�6    $	��0 5q�6.   $ ��Cache time out in secondes��6<   $��Center=�6a   $
��Left=�6|   ���	#FFEEEEEE6�   �� 5:      -   5        $��InputCacheTimeOut��5:   "   $	��600��6;   $
��10 0q�6F   $��5q�6T   5;      �� 5<      $ Vertical=�6!   .+�5=      �� $	��0 5q�6    $
��10 0q�6.   $��
Buffersize��6<   $��Center=�6Q   $
��Left=�6l   ���	#FFEEEEEE6�   �� 5>      -   5        $��InputBufferSize��5>   "   $
��1024��69   $
��10 0q�6E   $��5q�6S   5?      �� 5A      $ 
Horizontal=�6!   $
��Left=�6:   .+�5B      �� -   5        $��filterContent��5B   #   $��10q�68   $��0,5,0,0q�6D   $��Stretch=�6V   �� 5C      $��Filter Content��6    $��Center=�69   $
��Left=�6T   ���	#FFEEEEEE6o   5D      �� 5F      $ 
Horizontal=�6!   $
��Left=�6:   .+�5G      �� -	   5        $��cacheContent��5G   #   $��10q�67   $��0,5,0,0q�6C   $��Stretch=�6U    d� 	IsChecked$
 TrueY�6s   �� 5H      $��Gebruik Caching��6    $��Center=�6:   $
��Left=�6U   ���	#FFEEEEEE6p   5I      �� 5K      $ 
Horizontal=�6!   $
��Left=�6:   .+�5L      �� -
   5        $��basicAuthentication��5L   #   $��10q�6>   $��0,5,0,0q�6J   �� 5M      $��Basis Authenticatie��6!   $��Center=�6?   $
��Left=�6Z   ���	#FFEEEEEE6u   5N      �� 5P      $ 
Horizontal=�6!   $
��Left=�6:   .+�5Q      �� -   5        $��hideUserAgent��5Q   #   $��10q�68   $��0,5,0,0q�6D   �� 5R      $��Verberg UserAgent��6!   $��Center=�6=   $
��Left=�6X   ���	#FFEEEEEE6s   5S      �� 5U      $ 
Horizontal=�6!   $
��Left=�6:   .+�5V      �� -   5        $��logRequestHeader��5V   #   $��10q�6;   $��0,5,0,0q�6G   $
 TrueY�6Y   �� 5W      $��Log Request Headers��6!   $��Center=�6?   $
��Left=�6Z   ���	#FFEEEEEE6u   5X      �� 5Z      $ 
Horizontal=�6!   $
��Left=�6:   .+�5[      �� -   5        $��logResponseHeaders��5[   #   $��10q�6=   $��0,5,0,0q�6I   $
 TrueY�6[   �� 5\      $��Log Response Headers��6!   $��Center=�6@   $
��Left=�6[   ���	#FFEEEEEE6v   5]      �� 5_      -   5        $��StartStopButton��5_      $��Start / Stop proxy��64   $��10q�6Q   $��5q�6^   6i   �� 5`      -   5        $��ErrorTextBlock��5`       $�� ��66   ���	#FFE44141	 �� Padding$	 2q�6U   $��51��6a   $
��Wrap=�6m   5a      5b      5c      5d      5e      