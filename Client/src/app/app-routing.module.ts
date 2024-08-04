import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
import { ActionsComponent } from './actions/actions.component';
import { AuthorizationGuard } from './shared/guards/authorizations.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthorizationGuard],
    children: [
      {path: 'actions', component: ActionsComponent}
    ]
  },
  {path: 'account', loadChildren: () => import('./account/account.module').then(module => module.AccountModule)},
  {path: 'not-found', component: NotFoundComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
