import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './helpers/auth-guard';
import { UsersComponent } from './components/users/users.component';
import { RoleGuard } from './helpers/role-guard';
import { DevicesComponent } from './components/devices/devices.component';
import { ChatComponent } from './components/chat/chat.component';

const routes: Routes = [
  {
    path:'login',
    component: LoginComponent
  },
  {
    path:'users',
    component: UsersComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { 
      expectedRoles: ['Admin']
    }
  },
  {
    path: 'devices',
    component: DevicesComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { 
      expectedRoles: ['Admin', 'User']
    }
  },
  {
    path: 'chat',
    component: ChatComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { 
      expectedRoles: ['Admin', 'User']
    }
  },
  {
    path:'',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path:'**',
    redirectTo:'/login',
    pathMatch:'full'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
