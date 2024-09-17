from django.shortcuts import render

# Create your views here.

from django.shortcuts import render


def index(request):
    return render(request, 'index.html')


def account(request):
    return render(request, 'account.html')


def list(request):
    return render(request, 'list.html')
def accsettings(request):
    return render(request, 'accsettings.html')