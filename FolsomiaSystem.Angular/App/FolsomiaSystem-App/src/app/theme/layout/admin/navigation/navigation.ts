import {Injectable} from '@angular/core';

export interface NavigationItem {
  id: string;
  title: string;
  type: 'item' | 'collapse' | 'group';
  translate?: string;
  icon?: string;
  hidden?: boolean;
  url?: string;
  classes?: string;
  exactMatch?: boolean;
  external?: boolean;
  target?: boolean;
  breadcrumbs?: boolean;
  function?: any;
  badge?: {
    title?: string;
    type?: string;
  };
  children?: Navigation[];
}

export interface Navigation extends NavigationItem {
  children?: NavigationItem[];
}

const NavigationItems = [
  {
    id: 'navigation',
    title: '',
    type: 'group',
    icon: 'feather icon-monitor',
    children: [
      {
        id: 'dashboard',
        title: 'Contador de Folsomia',
        type: 'item',
        url: '/dashboard/default',
        icon: 'feather icon-loader',
        classes: 'nav-item'
      },
      {
        id: 'manual',
        title: 'Manual',
        type: 'item',
        url: '/manual',
        classes: 'nav-item',
        icon: 'feather icon-book'
      },
      {
        id: 'about',
        title: 'Sobre',
        type: 'item',
        url: '/about',
        classes: 'nav-item',
        icon: 'feather icon-info'
      }
    ]
  }

];

@Injectable()
export class NavigationItem {
  get() {
    return NavigationItems;
  }
}
