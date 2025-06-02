// Base styles
import './base.scss';

// Atoms (basic building blocks)
import './Components/Button/Button.js';
// Molecules (simple groups of UI elements functioning together as a unit)
import './Components/Alert/Alert.js';
import './Components/Breadcrumbs/Breadcrumbs.js';
import './Components/ContentList/ContentList.js';
import './Components/Heading/Heading.js';
import './Components/IdentityBar/IdentityBar.js';
import './Components/SkipLink/SkipLink.js';
// Organisms (complex UI components composed of groups of molecules and/or atoms)
import './Components/Header/Header.js';
import './Components/Footer/Footer.js';
import './Components/Pagination/Pagination.js';
import './Components/PrimaryNavigation/PrimaryNavigation.js';
import './Components/Table/Table.js';
import form from './Components/Form/Form.js';
import listFilter from './Components/ListFilter/ListFilter.js';

addEventListener('DOMContentLoaded', () => {
    form();
    listFilter();
});
