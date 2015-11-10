<?php
namespace BackendBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tbl_devices")
 */
class Device
{
    /**
     * @ORM\Column(type="integer")
     * @ORM\Id
     * @ORM\GeneratedValue(strategy="AUTO")
     */
    protected $id;
    
    /**
     * @ORM\Column(type="string", length=100)
     */
    protected $name;
    
    /**
     * @ORM\Column(type="integer", nullable=true)
     */
    protected $datamineChannel;
    
    /**
     * @ORM\Column(type="string")
     */
    protected $type;
    
 /**
     * @return the $id
     */
    public function getId()
    {
        return $this->id;
    }

 /**
     * @return the $name
     */
    public function getName()
    {
        return $this->name;
    }

 /**
     * @return the $datamineChannel
     */
    public function getDatamineChannel()
    {
        return $this->datamineChannel;
    }

 /**
     * @param field_type $id
     */
    public function setId($id)
    {
        $this->id = $id;
    }

 /**
     * @param field_type $name
     */
    public function setName($name)
    {
        $this->name = $name;
    }

 /**
     * @param field_type $datamineChannel
     */
    public function setDatamineChannel($datamineChannel)
    {
        $this->datamineChannel = $datamineChannel;
    }
    
 /**
     * @return the $type
     */
    public function getType()
    {
        return $this->type;
    }

 /**
     * @param field_type $type
     */
    public function setType($type)
    {
        $this->type = $type;
    }


    
    
    
}