import { Routes } from '@angular/router';
import { LandingPage } from './Components/landing-page/landing-page';
import { HomePage } from './Components/home-page/home-page';

export const routes: Routes = [
    {
        path: '',
        component: LandingPage
    },
    {
        path:'home',
        component: HomePage
    }
];
