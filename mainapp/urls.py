
from django.urls import path
from . import views

urlpatterns = [
    path('', views.index, name='index'),
    path('account/', views.account, name='account'),
    path('list/', views.list, name='list'),
    path('accsettings/', views.accsettings, name='accsettings'),
]


