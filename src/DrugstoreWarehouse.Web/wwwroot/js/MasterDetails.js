const contentHeight = calculateContentHeight();
document.querySelector('.master-details__master-list-container').setAttribute('style', `height:${contentHeight}px;  max-height:${contentHeight}`);
document.querySelector('.master-details__details-container').setAttribute('style', `height:${contentHeight}px; max-height:${contentHeight}`);
console.log(contentHeight)