import { RaspiDualPhotoWebpagePage } from './app.po';

describe('raspi-dual-photo-webpage App', () => {
  let page: RaspiDualPhotoWebpagePage;

  beforeEach(() => {
    page = new RaspiDualPhotoWebpagePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
