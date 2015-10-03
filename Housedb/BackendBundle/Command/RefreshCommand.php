<?php
namespace BackendBundle\Command;

use Symfony\Bundle\FrameworkBundle\Command\ContainerAwareCommand;
use Symfony\Component\Console\Output\OutputInterface;
use Symfony\Component\Console\Input\InputInterface;
use BackendBundle\Entity\VeraCacheRefresher;

class RefreshCommand extends ContainerAwareCommand{
    
    protected function configure(){
        $this
        ->setName('refreshCaches')
        ->setDescription('Refreshes all vera cached values')
        ;
    }
    
    protected function execute(InputInterface $input, OutputInterface $output){
        $em = $this->getContainer()->get('doctrine')->getManager();
        
        $refresher = new VeraCacheRefresher();
        $refresher->doRefresh($em);
        
        $output->writeln("Done refreshing");
    }
}